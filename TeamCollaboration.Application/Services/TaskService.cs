using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using TeamCollaboration.Application.DTOs;
using TeamCollaboration.Application.Mapping;
using TeamCollaboration.Application.RealTime;
using TeamCollaboration.Application.Validation;
using TeamCollaboration.Domain.Entities;
using TeamCollaboration.Domain.Repositories;
using DomainTaskStatus = TeamCollaboration.Domain.Enums.TaskStatus;

namespace TeamCollaboration.Application.Services;

/// <summary>
/// Coordinates task-related use cases between the UI and domain.
/// </summary>
public class TaskService : ITaskService
{
    private readonly ITaskRepository _repository;
    private readonly IHubContext<KanbanHub> _hubContext;
    private readonly ILogger<TaskService> _logger;

    public TaskService(
        ITaskRepository repository,
        IHubContext<KanbanHub> hubContext,
        ILogger<TaskService> logger)
    {
        _repository = repository;
        _hubContext = hubContext;
        _logger = logger;
    }

    public async Task<(TaskItemDto? task, IReadOnlyCollection<ValidationResult> errors)> CreateAsync(TaskItemDto request, CancellationToken cancellationToken = default)
    {
        var errors = TaskItemValidator.ValidateForCreate(request);
        if (errors.Count > 0)
        {
            return (null, errors);
        }

        var task = new TaskItem(request.BoardId, request.BoardColumnId, request.Title);
        task.UpdateDetails(request.Title, request.Description, request.DueAtUtc);
        task.AssignTo(request.Assignee);
        var status = Enum.IsDefined(typeof(DomainTaskStatus), request.Status) ? request.Status : DomainTaskStatus.ToDo;
        task.MoveTo(request.BoardColumnId, status, request.SortOrder);

        await _repository.AddAsync(task, cancellationToken);
        _logger.LogInformation("Created task {TaskId} in column {ColumnId}", task.Id, task.BoardColumnId);

        var dto = task.ToDto();
        await _hubContext.Clients.All.SendAsync("ReceiveTaskCreated", dto, cancellationToken);

        return (dto, Array.Empty<ValidationResult>());
    }

    public async Task<TaskItemDto?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var task = await _repository.GetByIdAsync(id, cancellationToken);
        return task?.ToDto();
    }

    public async Task<IReadOnlyCollection<TaskItemDto>> GetByBoardAsync(Guid boardId, CancellationToken cancellationToken = default)
    {
        var items = await _repository.GetByBoardAsync(boardId, cancellationToken);
        return items.Select(t => t.ToDto()).ToArray();
    }

    public async Task<bool> UpdateDetailsAsync(Guid id, string title, string? description, DateTime? dueAtUtc, CancellationToken cancellationToken = default)
    {
        var task = await _repository.GetByIdAsync(id, cancellationToken);
        if (task is null)
        {
            _logger.LogWarning("Attempted to update task {TaskId} but it does not exist", id);
            return false;
        }

        task.UpdateDetails(title, description, dueAtUtc);
        await _repository.UpdateAsync(task, cancellationToken);
        await _hubContext.Clients.All.SendAsync("ReceiveTaskUpdated", task.ToDto(), cancellationToken);
        return true;
    }

    public async Task<(bool success, IReadOnlyCollection<ValidationResult> errors)> MoveTaskAsync(MoveTaskRequest request, CancellationToken cancellationToken = default)
    {
        var errors = TaskItemValidator.ValidateForMove(request);
        if (errors.Count > 0)
        {
            return (false, errors);
        }

        var success = await _repository.UpdateTaskPositionAsync(
            request.TaskId,
            request.BoardId,
            request.TargetColumnId,
            request.TargetOrder,
            request.TargetStatus,
            cancellationToken);

        if (!success)
        {
            _logger.LogWarning(
                "Attempted to move task {TaskId} but it does not exist or belongs to another board",
                request.TaskId);
            return (false, Array.Empty<ValidationResult>());
        }

        await _hubContext.Clients.All.SendAsync(
            "ReceiveTaskMoved",
            request.TaskId,
            request.BoardId,
            request.TargetColumnId,
            request.TargetOrder,
            request.TargetStatus,
            cancellationToken);

        return (true, Array.Empty<ValidationResult>());
    }

    public async Task<bool> UpdateStatusAsync(Guid id, DomainTaskStatus status, CancellationToken cancellationToken = default)
    {
        var task = await _repository.GetByIdAsync(id, cancellationToken);
        if (task is null)
        {
            _logger.LogWarning("Attempted to update status for task {TaskId} but it does not exist", id);
            return false;
        }

        task.MoveTo(task.BoardColumnId, status, task.SortOrder);
        await _repository.UpdateAsync(task, cancellationToken);
        await _hubContext.Clients.All.SendAsync("ReceiveTaskStatusChanged", task.ToDto(), cancellationToken);
        return true;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _repository.DeleteAsync(id, cancellationToken);
        await _hubContext.Clients.All.SendAsync("ReceiveTaskDeleted", id, cancellationToken);
    }
}


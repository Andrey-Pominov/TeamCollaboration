using System.ComponentModel.DataAnnotations;
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

public class TaskService(
    ITaskRepository repository,
    IHubContext<KanbanHub> hubContext,
    ILogger<TaskService> logger)
    : ITaskService
{
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

        await repository.AddAsync(task, cancellationToken);
        logger.LogInformation("Created task {TaskId} in column {ColumnId}", task.Id, task.BoardColumnId);

        var dto = task.ToDto();
        await hubContext.Clients.All.SendAsync("ReceiveTaskCreated", dto, cancellationToken);

        return (dto, []);
    }

    public async Task<TaskItemDto?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var task = await repository.GetByIdAsync(id, cancellationToken);
        return task?.ToDto();
    }

    public async Task<IReadOnlyCollection<TaskItemDto>> GetByBoardAsync(Guid boardId, CancellationToken cancellationToken = default)
    {
        var items = await repository.GetByBoardAsync(boardId, cancellationToken);
        return items.Select(t => t.ToDto()).ToArray();
    }

    public async Task<bool> UpdateDetailsAsync(Guid id, string title, string? description, DateTime? dueAtUtc, CancellationToken cancellationToken = default)
    {
        var task = await repository.GetByIdAsync(id, cancellationToken);
        if (task is null)
        {
            logger.LogWarning("Attempted to update task {TaskId} but it does not exist", id);
            return false;
        }

        task.UpdateDetails(title, description, dueAtUtc);
        await repository.UpdateAsync(task, cancellationToken);
        await hubContext.Clients.All.SendAsync("ReceiveTaskUpdated", task.ToDto(), cancellationToken);
        return true;
    }

    public async Task<(bool success, IReadOnlyCollection<ValidationResult> errors)> MoveTaskAsync(MoveTaskRequest request, CancellationToken cancellationToken = default)
    {
        var errors = TaskItemValidator.ValidateForMove(request);
        if (errors.Count > 0)
        {
            return (false, errors);
        }

        var success = await repository.UpdateTaskPositionAsync(
            request.TaskId,
            request.BoardId,
            request.TargetColumnId,
            request.TargetOrder,
            request.TargetStatus,
            cancellationToken);

        if (!success)
        {
            logger.LogWarning(
                "Attempted to move task {TaskId} but it does not exist or belongs to another board",
                request.TaskId);
            return (false, []);
        }

        await hubContext.Clients.All.SendAsync(
            "ReceiveTaskMoved",
            request.TaskId,
            request.BoardId,
            request.TargetColumnId,
            request.TargetOrder,
            request.TargetStatus,
            cancellationToken);

        return (true, []);
    }

    public async Task<bool> UpdateStatusAsync(Guid id, DomainTaskStatus status, CancellationToken cancellationToken = default)
    {
        var task = await repository.GetByIdAsync(id, cancellationToken);
        if (task is null)
        {
            logger.LogWarning("Attempted to update status for task {TaskId} but it does not exist", id);
            return false;
        }

        task.MoveTo(task.BoardColumnId, status, task.SortOrder);
        await repository.UpdateAsync(task, cancellationToken);
        await hubContext.Clients.All.SendAsync("ReceiveTaskStatusChanged", task.ToDto(), cancellationToken);
        return true;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await repository.DeleteAsync(id, cancellationToken);
        await hubContext.Clients.All.SendAsync("ReceiveTaskDeleted", id, cancellationToken);
    }
}


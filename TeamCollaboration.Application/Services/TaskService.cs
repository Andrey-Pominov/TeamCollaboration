using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.Extensions.Logging;
using TeamCollaboration.Application.DTOs;
using TeamCollaboration.Application.Mapping;
using TeamCollaboration.Application.Validation;
using TeamCollaboration.Domain.Entities;
using DomainTaskStatus = TeamCollaboration.Domain.Enums.TaskStatus;
using TeamCollaboration.Domain.Repositories;

namespace TeamCollaboration.Application.Services;

/// <summary>
/// Coordinates task-related use cases between the UI and domain.
/// </summary>
public class TaskService
{
    private readonly ITaskRepository _repository;
    private readonly ILogger<TaskService> _logger;

    public TaskService(ITaskRepository repository, ILogger<TaskService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<(TaskDto? task, IReadOnlyCollection<ValidationResult> errors)> CreateAsync(TaskDto request, CancellationToken cancellationToken = default)
    {
        var errors = TaskValidator.ValidateForCreate(request);
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

        return (task.ToDto(), Array.Empty<ValidationResult>());
    }

    public async Task<TaskDto?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var task = await _repository.GetByIdAsync(id, cancellationToken);
        return task?.ToDto();
    }

    public async Task<IReadOnlyCollection<TaskDto>> GetByBoardAsync(Guid boardId, CancellationToken cancellationToken = default)
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
        return true;
    }

    public async Task<bool> MoveAsync(Guid id, Guid columnId, DomainTaskStatus status, int sortOrder, CancellationToken cancellationToken = default)
    {
        var task = await _repository.GetByIdAsync(id, cancellationToken);
        if (task is null)
        {
            _logger.LogWarning("Attempted to move task {TaskId} but it does not exist", id);
            return false;
        }

        task.MoveTo(columnId, status, sortOrder);
        await _repository.UpdateAsync(task, cancellationToken);
        return true;
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _repository.DeleteAsync(id, cancellationToken);
    }
}


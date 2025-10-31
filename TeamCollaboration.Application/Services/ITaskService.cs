using System.ComponentModel.DataAnnotations;
using TeamCollaboration.Application.DTOs;
using TaskStatus = TeamCollaboration.Domain.Enums.TaskStatus;

namespace TeamCollaboration.Application.Services;

/// <summary>
/// Exposes task-related operations to the presentation layer.
/// </summary>
public interface ITaskService
{
    Task<TaskItemDto?> GetAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<TaskItemDto>> GetByBoardAsync(Guid boardId, CancellationToken cancellationToken = default);

    Task<(TaskItemDto? task, IReadOnlyCollection<ValidationResult> errors)> CreateAsync(TaskItemDto request, CancellationToken cancellationToken = default);

    Task<bool> UpdateDetailsAsync(Guid id, string title, string? description, DateTime? dueAtUtc, CancellationToken cancellationToken = default);

    Task<(bool success, IReadOnlyCollection<ValidationResult> errors)> MoveTaskAsync(MoveTaskRequest request, CancellationToken cancellationToken = default);

    Task<bool> UpdateStatusAsync(Guid id, TaskStatus status, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}


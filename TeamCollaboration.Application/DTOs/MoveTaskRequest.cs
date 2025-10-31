using System.ComponentModel.DataAnnotations;
using TaskStatus = TeamCollaboration.Domain.Enums.TaskStatus;

namespace TeamCollaboration.Application.DTOs;

/// <summary>
/// Represents the data needed to move a task between columns in the UI.
/// </summary>
public sealed class MoveTaskRequest
{
    [Required]
    public Guid TaskId { get; init; }

    [Required]
    public Guid BoardId { get; init; }

    [Required]
    public Guid TargetColumnId { get; init; }

    [Range(0, int.MaxValue)]
    public int TargetOrder { get; init; }

    public TaskStatus TargetStatus { get; init; } = TaskStatus.ToDo;
}


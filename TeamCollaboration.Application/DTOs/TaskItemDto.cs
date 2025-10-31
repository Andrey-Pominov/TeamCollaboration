using TaskStatus = TeamCollaboration.Domain.Enums.TaskStatus;

namespace TeamCollaboration.Application.DTOs;

/// <summary>
/// Transport-friendly representation of a task item for API/UI consumption.
/// </summary>
public sealed class TaskItemDto
{
    public Guid Id { get; init; }

    public string Title { get; init; } = string.Empty;

    public string? Description { get; init; }

    public TaskStatus Status { get; init; }

    public Guid BoardId { get; init; }

    public Guid BoardColumnId { get; init; }

    public int SortOrder { get; init; }

    public string? Assignee { get; init; }

    public DateTime CreatedAtUtc { get; init; }

    public DateTime? DueAtUtc { get; init; }

    public DateTime? CompletedAtUtc { get; init; }
}


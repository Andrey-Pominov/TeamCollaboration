using TeamCollaboration.Application.DTOs;
using TeamCollaboration.Domain.Entities;

namespace TeamCollaboration.Application.Mapping;

/// <summary>
/// Provides conversions between domain entities and DTOs.
/// </summary>
public static class TaskMappingExtensions
{
    public static TaskItemDto ToDto(this TaskItem task)
    {
        return new TaskItemDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            BoardId = task.BoardId,
            BoardColumnId = task.BoardColumnId,
            SortOrder = task.SortOrder,
            Assignee = task.Assignee,
            CreatedAtUtc = task.CreatedAtUtc,
            DueAtUtc = task.DueAtUtc,
            CompletedAtUtc = task.CompletedAtUtc
        };
    }
}


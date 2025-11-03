using System.ComponentModel.DataAnnotations;
using TeamCollaboration.Application.DTOs;

namespace TeamCollaboration.Application.Validation;

public static class TaskItemValidator
{
    public static IReadOnlyCollection<ValidationResult> ValidateForCreate(TaskItemDto candidate)
    {
        var results = new List<ValidationResult>();

        if (string.IsNullOrWhiteSpace(candidate.Title))
        {
            results.Add(new ValidationResult("Title is required", new[] { nameof(candidate.Title) }));
        }

        if (candidate.BoardColumnId == Guid.Empty)
        {
            results.Add(new ValidationResult("A valid board column is required", new[] { nameof(candidate.BoardColumnId) }));
        }

        if (candidate.BoardId == Guid.Empty)
        {
            results.Add(new ValidationResult("A valid board is required", new[] { nameof(candidate.BoardId) }));
        }

        if (candidate.DueAtUtc.HasValue && candidate.DueAtUtc.Value < DateTime.UtcNow.Date)
        {
            results.Add(new ValidationResult("Due date cannot be in the past", new[] { nameof(candidate.DueAtUtc) }));
        }

        return results;
    }

    public static IReadOnlyCollection<ValidationResult> ValidateForMove(MoveTaskRequest request)
    {
        var results = new List<ValidationResult>();

        if (request.TaskId == Guid.Empty)
        {
            results.Add(new ValidationResult("TaskId is required", new[] { nameof(request.TaskId) }));
        }

        if (request.BoardId == Guid.Empty)
        {
            results.Add(new ValidationResult("BoardId is required", new[] { nameof(request.BoardId) }));
        }

        if (request.TargetColumnId == Guid.Empty)
        {
            results.Add(new ValidationResult("Target column is required", new[] { nameof(request.TargetColumnId) }));
        }

        if (request.TargetOrder < 0)
        {
            results.Add(new ValidationResult("Target order must be non-negative", new[] { nameof(request.TargetOrder) }));
        }

        return results;
    }
}


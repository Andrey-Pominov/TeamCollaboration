using System.ComponentModel.DataAnnotations;
using TeamCollaboration.Application.DTOs;

namespace TeamCollaboration.Application.Validation;

/// <summary>
/// Lightweight validation rules that can be applied before command execution.
/// </summary>
public static class TaskValidator
{
    public static IReadOnlyCollection<ValidationResult> ValidateForCreate(TaskDto candidate)
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
}


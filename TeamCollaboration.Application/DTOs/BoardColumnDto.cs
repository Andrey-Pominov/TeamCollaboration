namespace TeamCollaboration.Application.DTOs;

/// <summary>
/// Transport-friendly representation of a board column.
/// </summary>
public sealed class BoardColumnDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }

    public Guid BoardId { get; init; }

    public int SortOrder { get; init; }

    public bool IsArchived { get; init; }

    public DateTime CreatedAtUtc { get; init; }
}


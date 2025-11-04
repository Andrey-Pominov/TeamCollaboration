using DomainTaskStatus = TeamCollaboration.Domain.Enums.TaskStatus;

namespace TeamCollaboration.Domain.Entities;

public class TaskItem
{
    public TaskItem(Guid boardId, Guid boardColumnId, string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Title must be provided", nameof(title));
        }

        Id = Guid.NewGuid();
        BoardId = boardId;
        BoardColumnId = boardColumnId;
        Title = title.Trim();
        Status = DomainTaskStatus.ToDo;
        CreatedAtUtc = DateTime.UtcNow;
    }

    private TaskItem() { }

    public Guid Id { get; private set; }

    public string Title { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    public DomainTaskStatus Status { get; private set; }

    public Guid BoardId { get; private set; }

    public Guid BoardColumnId { get; private set; }

    public BoardColumn? Column { get; private set; }

    public int SortOrder { get; private set; }

    public string? Assignee { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    public DateTime? DueAtUtc { get; private set; }

    public DateTime? CompletedAtUtc { get; private set; }

    public void UpdateDetails(string title, string? description, DateTime? dueAtUtc)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Title must be provided", nameof(title));
        }

        Title = title.Trim();
        Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
        DueAtUtc = dueAtUtc;
    }

    public void AssignTo(string? assignee)
    {
        Assignee = string.IsNullOrWhiteSpace(assignee) ? null : assignee.Trim();
    }

    public void MoveTo(Guid destinationColumnId, DomainTaskStatus status, int sortOrder)
    {
        BoardColumnId = destinationColumnId;
        Status = status;
        SortOrder = sortOrder;
        CompletedAtUtc = status == DomainTaskStatus.Done ? DateTime.UtcNow : null;
    }
}


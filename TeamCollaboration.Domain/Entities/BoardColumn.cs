namespace TeamCollaboration.Domain.Entities;

public class BoardColumn
{
    public BoardColumn(Guid boardId, string name, int sortOrder)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Column name must be provided", nameof(name));
        }

        Id = Guid.NewGuid();
        BoardId = boardId;
        Name = name.Trim();
        SortOrder = sortOrder;
        CreatedAtUtc = DateTime.UtcNow;
    }

    private BoardColumn() { }

    public Guid Id { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    public Guid BoardId { get; private set; }

    public int SortOrder { get; private set; }

    public bool IsArchived { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    public ICollection<TaskItem> Tasks { get; private set; } = new List<TaskItem>();

    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Column name must be provided", nameof(name));
        }

        Name = name.Trim();
    }

    public void UpdateDescription(string? description)
    {
        Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
    }

    public void Archive()
    {
        IsArchived = true;
    }

    public void Restore()
    {
        IsArchived = false;
    }
}


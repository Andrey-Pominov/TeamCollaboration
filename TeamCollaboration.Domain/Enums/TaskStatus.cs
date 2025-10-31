namespace TeamCollaboration.Domain.Enums;

/// <summary>
/// Describes the lifecycle states a task item can be in within a kanban board.
/// </summary>
public enum TaskStatus
{
    ToDo = 0,
    InProgress = 1,
    Blocked = 2,
    Done = 3
}


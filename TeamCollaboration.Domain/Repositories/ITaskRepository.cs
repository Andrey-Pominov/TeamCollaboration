using TeamCollaboration.Domain.Entities;

namespace TeamCollaboration.Domain.Repositories;

public interface ITaskRepository
{
    Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<TaskItem>> GetByBoardAsync(Guid boardId, CancellationToken cancellationToken = default);

    Task AddAsync(TaskItem task, CancellationToken cancellationToken = default);

    Task UpdateAsync(TaskItem task, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    Task<bool> UpdateTaskPositionAsync(
        Guid taskId,
        Guid boardId,
        Guid newColumnId,
        int newOrder,
        TeamCollaboration.Domain.Enums.TaskStatus targetStatus,
        CancellationToken cancellationToken = default);
}


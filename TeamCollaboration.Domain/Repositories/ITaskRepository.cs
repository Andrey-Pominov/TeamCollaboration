using TeamCollaboration.Domain.Entities;

namespace TeamCollaboration.Domain.Repositories;

/// <summary>
/// Contract for persistence operations that manage <see cref="TaskItem"/> aggregates.
/// </summary>
public interface ITaskRepository
{
    Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<TaskItem>> GetByBoardAsync(Guid boardId, CancellationToken cancellationToken = default);

    Task AddAsync(TaskItem task, CancellationToken cancellationToken = default);

    Task UpdateAsync(TaskItem task, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}


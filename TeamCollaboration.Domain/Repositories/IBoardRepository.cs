using TeamCollaboration.Domain.Entities;

namespace TeamCollaboration.Domain.Repositories;

public interface IBoardRepository
{
    Task<IReadOnlyCollection<BoardColumn>> GetColumnsAsync(Guid boardId, CancellationToken cancellationToken = default);

    Task<BoardColumn?> GetColumnAsync(Guid columnId, CancellationToken cancellationToken = default);

    Task AddColumnAsync(BoardColumn column, CancellationToken cancellationToken = default);

    Task UpdateColumnAsync(BoardColumn column, CancellationToken cancellationToken = default);

    Task DeleteColumnAsync(Guid columnId, CancellationToken cancellationToken = default);
}


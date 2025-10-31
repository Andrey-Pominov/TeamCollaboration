using System.Linq;
using Microsoft.EntityFrameworkCore;
using TeamCollaboration.Domain.Entities;
using TeamCollaboration.Domain.Repositories;
using TeamCollaboration.Infrastructure.Persistence;

namespace TeamCollaboration.Infrastructure.Repositories;

/// <summary>
/// Entity Framework Core implementation of <see cref="IBoardRepository"/>.
/// </summary>
public class BoardRepository(ApplicationDbContext dbContext) : IBoardRepository
{
    public async Task AddColumnAsync(BoardColumn column, CancellationToken cancellationToken = default)
    {
        await dbContext.Columns.AddAsync(column, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteColumnAsync(Guid columnId, CancellationToken cancellationToken = default)
    {
        var entity = await dbContext.Columns.FirstOrDefaultAsync(c => c.Id == columnId, cancellationToken);
        if (entity is null)
        {
            return;
        }

        dbContext.Columns.Remove(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<BoardColumn?> GetColumnAsync(Guid columnId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Columns.AsNoTracking().FirstOrDefaultAsync(c => c.Id == columnId, cancellationToken);
    }

    public async Task<IReadOnlyCollection<BoardColumn>> GetColumnsAsync(Guid boardId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Columns
            .AsNoTracking()
            .Where(c => c.BoardId == boardId && !c.IsArchived)
            .OrderBy(c => c.SortOrder)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateColumnAsync(BoardColumn column, CancellationToken cancellationToken = default)
    {
        dbContext.Columns.Update(column);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}


using Microsoft.EntityFrameworkCore;
using TeamCollaboration.Domain.Entities;
using TeamCollaboration.Domain.Repositories;
using TeamCollaboration.Infrastructure.Persistence;

namespace TeamCollaboration.Infrastructure.Repositories;


public class TaskRepository(ApplicationDbContext dbContext) : ITaskRepository
{
    public async Task AddAsync(TaskItem task, CancellationToken cancellationToken = default)
    {
        await dbContext.Tasks.AddAsync(task, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await dbContext.Tasks.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        if (entity is null)
        {
            return;
        }

        dbContext.Tasks.Remove(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<TaskItem>> GetByBoardAsync(Guid boardId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Tasks
            .AsNoTracking()
            .Where(t => t.BoardId == boardId)
            .OrderBy(t => t.SortOrder)
            .ThenBy(t => t.CreatedAtUtc)
            .ToListAsync(cancellationToken);
    }

    public async Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Tasks.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task UpdateAsync(TaskItem task, CancellationToken cancellationToken = default)
    {
        dbContext.Tasks.Update(task);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> UpdateTaskPositionAsync(
        Guid taskId,
        Guid boardId,
        Guid newColumnId,
        int newOrder,
        Domain.Enums.TaskStatus targetStatus,
        CancellationToken cancellationToken = default)
    {
        var entity = await dbContext.Tasks.FirstOrDefaultAsync(
            t => t.Id == taskId && t.BoardId == boardId,
            cancellationToken);

        if (entity is null)
        {
            return false;
        }

        entity.MoveTo(newColumnId, targetStatus, newOrder);
        dbContext.Tasks.Update(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}


using System.Linq.Expressions;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.TaskBoardService.Core.Aggregates;
using TaskManagementSystem.TaskBoardService.Core.Interfaces.Repository;
using TaskManagementSystem.TaskBoardService.Core.Models;
using TaskManagementSystem.TaskBoardService.Infrastructure.DataAccess.ORM;

namespace TaskManagementSystem.TaskBoardService.Infrastructure.DataAccess.Repositories;


public class TaskBoardRepository : ITaskBoardRepository
{
    private readonly ApplicationDbContext _dbContext;

    public TaskBoardRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<TaskBoardColumnModel>> GetColumnsAsync(Guid taskBoardId, CancellationToken cancellationToken)
    {
        return await _dbContext.TaskBoards
            .Where(b => b.Id == taskBoardId)
            .SelectMany(b => b.Columns)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TaskBoardAggregate>> GetByIdsAsync(IEnumerable<Guid> ids, bool detailed, CancellationToken cancellationToken)
    {
        var query = _dbContext.TaskBoards
            .Where(b => ids.Contains(b.Id))
            .AsNoTracking();

        if (detailed)
            query.Include(b => b.Columns);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<TaskBoardAggregate?> GetByColumnIdAsync(Guid columnId, CancellationToken cancellationToken)
    {
        return await _dbContext.TaskBoards
            .Where(b => b.Columns.Any(c => c.Id == columnId))
            .Include(b => b.Columns)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<TaskBoardAggregate?> GetByIdAsync(Guid taskBoardId, CancellationToken cancellationToken)
    {
        return await _dbContext.TaskBoards
            .Where(b => b.Id == taskBoardId)
            .Include(b => b.Columns)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task CreateAsync(TaskBoardAggregate taskBoard, CancellationToken cancellationToken)
    {
        await _dbContext.TaskBoards.AddAsync(taskBoard, cancellationToken);
    }

    public void UpdateAsync(TaskBoardAggregate taskBoard, CancellationToken cancellationToken)
    {
        _dbContext.TaskBoards.Update(taskBoard);
    }

    public async Task DeleteAsync(Guid taskBoardId, CancellationToken cancellationToken)
    {
        var result = await _dbContext.TaskBoards.Where(b => b.Id == taskBoardId)
            .ExecuteDeleteAsync(cancellationToken);

        if (result != 1)
            throw new RuntimeBinderException($"Failed to delete task board. Expected 1 row affected, but got {result}.");
    }

    public async Task<bool> ExistsAsync(Expression<Func<TaskBoardAggregate, bool>> predicate, CancellationToken cancellationToken)
    {
        return await _dbContext.TaskBoards
            .AnyAsync(predicate, cancellationToken);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }
}

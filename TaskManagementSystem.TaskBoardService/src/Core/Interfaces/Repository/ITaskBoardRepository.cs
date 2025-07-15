using System.Linq.Expressions;
using TaskManagementSystem.TaskBoardService.Core.Aggregates;

namespace TaskManagementSystem.TaskBoardService.Core.Interfaces.Repository;


public interface ITaskBoardRepository
{
    Task<IEnumerable<TaskBoardAggregate>> GetAllAsync(Guid organizationId, CancellationToken cancellationToken);
    Task<TaskBoardAggregate?> GetByIdAsync(Guid taskBoardId, CancellationToken cancellationToken);
    Task CreateAsync(TaskBoardAggregate taskBoard, CancellationToken cancellationToken);
    Task UpdateAsync(TaskBoardAggregate taskBoard, CancellationToken cancellationToken);
    Task DeleteAsync(Guid taskBoardId, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(Expression<Func<TaskBoardAggregate, bool>> predicate, CancellationToken cancellationToken);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}

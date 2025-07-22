using System.Linq.Expressions;
using TaskManagementSystem.TaskBoardService.Core.Aggregates;
using TaskManagementSystem.TaskBoardService.Core.Models;

namespace TaskManagementSystem.TaskBoardService.Core.Interfaces.Repository;


public interface ITaskBoardRepository
{
    Task<IEnumerable<TaskBoardAggregate>> GetByIdsAsync(IEnumerable<Guid> ids, bool detailed, CancellationToken cancellationToken);
    Task<TaskBoardAggregate?> GetByIdAsync(Guid taskBoardId, CancellationToken cancellationToken);
    Task<TaskBoardAggregate?> GetByColumnIdAsync(Guid columnId, CancellationToken cancellationToken);
    Task CreateAsync(TaskBoardAggregate taskBoard, CancellationToken cancellationToken);
    Task<IEnumerable<TaskBoardColumnModel>> GetColumnsAsync(Guid taskBoardId, CancellationToken cancellationToken);
    void Update(TaskBoardAggregate taskBoard);
    Task DeleteAsync(Guid taskBoardId, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(Expression<Func<TaskBoardAggregate, bool>> predicate, CancellationToken cancellationToken);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}

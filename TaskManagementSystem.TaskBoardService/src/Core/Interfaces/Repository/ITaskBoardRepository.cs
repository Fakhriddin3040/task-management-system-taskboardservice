using TaskManagementSystem.TaskBoardService.Core.Aggregates;

namespace TaskManagementSystem.TaskBoardService.Core.Interfaces.Repository;


public interface ITaskBoardRepository
{
    Task<IEnumerable<TaskBoardAggregate>> GetAllAsync(Guid organizationId, CancellationToken cancellationToken);
    Task<TaskBoardAggregate> GetByIdAsync(Guid taskBoardId, Guid taskId, CancellationToken cancellationToken);
    Task CreateAsync(TaskBoardAggregate taskBoard, CancellationToken cancellationToken);
    Task UpdateAsync(TaskBoardAggregate taskBoard, CancellationToken cancellationToken);
    Task DeleteAsync(Guid taskBoardId);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}

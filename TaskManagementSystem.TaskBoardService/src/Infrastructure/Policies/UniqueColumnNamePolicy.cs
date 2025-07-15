using TaskManagementSystem.TaskBoardService.Core.Interfaces.Policies;
using TaskManagementSystem.TaskBoardService.Core.Interfaces.Repository;

namespace TaskManagementSystem.TaskBoardService.Infrastructure.Policies;


public class UniqueColumnNamePolicy : IUniqueTaskBoardColumnNamePolicy
{
    private readonly ITaskBoardRepository _taskBoardRepository;

    public UniqueColumnNamePolicy(ITaskBoardRepository taskBoardRepository)
    {
        _taskBoardRepository = taskBoardRepository;
    }

    public async Task<bool> IsUniqueAsync(string columnName, Guid taskBoardId, CancellationToken cancellationToken, Guid? columnId)
    {
        return !await _taskBoardRepository.ExistsAsync(b =>
            b.Id == taskBoardId &&
            b.Columns.Any(c =>
                c.Name == columnName &&
                (columnId == null || c.Id != columnId)
            ), cancellationToken);
    }
}

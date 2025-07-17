namespace TaskManagementSystem.TaskBoardService.Core.Interfaces.Policies;


public interface IUniqueColumnNamePolicy
{
    Task<bool> IsUniqueAsync(string columnName, Guid taskBoardId, CancellationToken cancellationToken,Guid? columnId);
}

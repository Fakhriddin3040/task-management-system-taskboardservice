namespace TaskManagementSystem.TaskBoardService.Core.Interfaces.Policies;


public interface IUniqueTaskBoardColumnNamePolicy
{
    Task<bool> IsUniqueAsync(string columnName, Guid taskBoardId, CancellationToken cancellationToken,Guid? columnId);
}

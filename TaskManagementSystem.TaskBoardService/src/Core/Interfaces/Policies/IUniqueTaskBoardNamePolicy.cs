namespace TaskManagementSystem.TaskBoardService.Core.Interfaces.Policies;


public interface IUniqueTaskBoardNamePolicy
{
    Task<bool> IsUnique(string name, CancellationToken cancellationToken);
}

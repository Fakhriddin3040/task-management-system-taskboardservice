namespace TaskManagementSystem.TaskBoardService.Core.Interfaces.Policies;


public interface IUniqueBoardNamePolicy
{
    Task<bool> IsUnique(string name, CancellationToken cancellationToken);
}

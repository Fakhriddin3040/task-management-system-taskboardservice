namespace TaskManagementSystem.TaskBoardService.Core.Interfaces.Policies;


public interface IValidColumnNamePolicy
{
    bool IsValid(string name);
}

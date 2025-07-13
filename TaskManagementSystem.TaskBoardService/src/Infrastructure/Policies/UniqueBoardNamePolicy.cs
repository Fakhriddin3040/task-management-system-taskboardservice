using TaskManagementSystem.TaskBoardService.Core.Interfaces.Policies;

namespace TaskManagementSystem.TaskBoardService.Infrastructure.Policies;


public class UniqueBoardNamePolicy : IUniqueTaskBoardNamePolicy
{
    public Task<bool> IsUnique(string name)
    {
        return Task.FromResult(true);
    }
}

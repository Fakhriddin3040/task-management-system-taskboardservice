using TaskManagementSystem.TaskBoardService.Core.Interfaces.Policies;

namespace TaskManagementSystem.TaskBoardService.Infrastructure.Policies;


public class UniqueBoardNamePolicy : IUniqueBoardNamePolicy
{
    public Task<bool> IsUnique(string name, CancellationToken cancellationToken)
    {
        return Task.FromResult(true);
    }
}

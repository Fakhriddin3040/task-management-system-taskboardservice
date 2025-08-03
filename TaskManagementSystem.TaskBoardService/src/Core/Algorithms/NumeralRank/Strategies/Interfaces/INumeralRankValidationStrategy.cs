namespace TaskManagementSystem.TaskBoardService.Core.Algorithms.NumeralRank.Interfaces;


public interface INumeralRankValidationStrategy
{
    Task<bool> ValidateAsync(Guid boardId, NumeralRankContext context, CancellationToken cancellationToken);
    bool CanHandle(NumeralRankContext rankContext);
}

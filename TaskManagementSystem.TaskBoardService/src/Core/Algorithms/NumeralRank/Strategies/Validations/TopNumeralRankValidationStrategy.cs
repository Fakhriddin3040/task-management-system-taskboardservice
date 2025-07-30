using TaskManagementSystem.TaskBoardService.Core.Algorithms.NumeralRank.Interfaces;

namespace TaskManagementSystem.TaskBoardService.Core.Algorithms.NumeralRank.Strategies.Validations;


public class TopNumeralRankValidationStrategy : INumeralRankValidationStrategy
{

    public Task<bool> ValidateAsync(NumeralRankContext numeralRankContext)
    {
        throw new NotImplementedException();
    }
    public bool CanHandle(NumeralRankContext numeralRankContext)
    {
        return numeralRankContext.IsToTop;
    }
}

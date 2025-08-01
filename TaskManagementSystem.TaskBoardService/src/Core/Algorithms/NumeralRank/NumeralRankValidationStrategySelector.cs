using TaskManagementSystem.TaskBoardService.Core.Algorithms.NumeralRank.Interfaces;

namespace TaskManagementSystem.TaskBoardService.Core.Algorithms.NumeralRank;


public class NumeralRankValidationStrategySelector : INumeralRankValidationStrategySelector
{
    private readonly IEnumerable<INumeralRankValidationStrategy> _strategies;

    public NumeralRankValidationStrategySelector(IEnumerable<INumeralRankValidationStrategy> strategies)
    {
        _strategies = strategies;
    }

    public INumeralRankValidationStrategy GetValidationStrategy(NumeralRankContext context)
    {
        foreach (var strategy in _strategies)
            if (strategy.CanHandle(context))
            {
                return strategy;
            }
        throw new InvalidOperationException("No suitable validation strategy found for the given context.");
    }
}

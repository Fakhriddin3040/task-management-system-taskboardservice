using TaskManagementSystem.TaskBoardService.Core.Algorithms.NumeralRank;
using TaskManagementSystem.TaskBoardService.Core.Algorithms.NumeralRank.Interfaces;

public class NumeralRankStrategySelector : INumeralRankStrategySelector
{
    private readonly IEnumerable<INumeralRankStrategy> _strategies;

    public NumeralRankStrategySelector(IEnumerable<INumeralRankStrategy> strategies)
    {
        _strategies = strategies;
    }

    public INumeralRankStrategy GetStrategy(NumeralRankContext context)
    {
        return _strategies.First(s => s.CanHandle(context));
    }
}

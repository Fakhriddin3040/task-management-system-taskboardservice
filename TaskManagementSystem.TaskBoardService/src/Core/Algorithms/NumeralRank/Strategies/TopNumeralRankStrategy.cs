using TaskManagementSystem.TaskBoardService.Core.Algorithms.NumeralRank.Interfaces;

namespace TaskManagementSystem.TaskBoardService.Core.Algorithms.NumeralRank.Strategies;


public class TopNumeralRankStrategy : INumeralRankStrategy
{
    public NumeralRankResult GenerateRank(NumeralRankContext context)
    {
        return new(
            rank: context.NextRank / 2);
    }
    public bool CanHandle(NumeralRankContext context)
    {
        return context.IsToTop;
    }
}

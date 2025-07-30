using TaskManagementSystem.TaskBoardService.Core.Algorithms.NumeralRank.Interfaces;

namespace TaskManagementSystem.TaskBoardService.Core.Algorithms.NumeralRank.Strategies;


public class EndNumeralRankStrategy : INumeralRankStrategy
{

    public NumeralRankResult GenerateRank(NumeralRankContext context)
    {
        return new(
            rank: context.PreviousRank + NumeralRankOptions.DefaultStep);
    }
    public bool CanHandle(NumeralRankContext context)
    {
        return context.IsToEnd;
    }
}

using TaskManagementSystem.SharedLib.Algorithms.NumeralRank;
using TaskManagementSystem.TaskBoardService.Core.Algorithms.NumeralRank.Interfaces;

namespace TaskManagementSystem.TaskBoardService.Core.Algorithms.NumeralRank.Strategies;


public class BetweenNumeralRankStrategy : INumeralRankStrategy
{

    public NumeralRankResult GenerateRank(NumeralRankContext context)
    {
        var needReorder = context.NextRank - context.PreviousRank < NumeralRankOptions.MinGap;

        return new(
            rank: needReorder
            ? NumeralRankOptions.NeedReordering
            : (context.PreviousRank + context.NextRank) / 2);
    }

    public bool CanHandle(NumeralRankContext context)
    {
        return context.IsBetween;
    }
}

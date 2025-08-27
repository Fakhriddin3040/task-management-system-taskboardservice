using TaskManagementSystem.SharedLib.Algorithms.NumeralRank;
using TaskManagementSystem.TaskBoardService.Core.Algorithms.NumeralRank.Interfaces;
using NumeralRankContext = TaskManagementSystem.TaskBoardService.Core.Algorithms.NumeralRank.NumeralRankContext;
using NumeralRankResult = TaskManagementSystem.TaskBoardService.Core.Algorithms.NumeralRank.NumeralRankResult;

namespace TaskManagementSystem.TaskBoardService.Core.Algorithms.NumeralRank.Strategies;


public class BetweenNumeralRankStrategy : INumeralRankStrategy
{

    public NumeralRankResult GenerateRank(NumeralRankContext context)
    {
        var needReorder = context.NextRank - context.PreviousRank < NumeralRankOptions.MinGap;

        return needReorder
            ? NumeralRankResult.ForReorder()
            : new NumeralRankResult(
                (context.PreviousRank + context.NextRank) / 2);
    }

    public bool CanHandle(NumeralRankContext context)
    {
        return context.IsBetween;
    }
}

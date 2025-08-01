using TaskManagementSystem.SharedLib.Algorithms.NumeralRank;
using TaskManagementSystem.TaskBoardService.Core.Algorithms.NumeralRank.Interfaces;

namespace TaskManagementSystem.TaskBoardService.Core.Algorithms.NumeralRank.Strategies;


public class TopNumeralRankStrategy : INumeralRankStrategy
{
    public NumeralRankResult GenerateRank(NumeralRankContext context)
    {
        var needReorder = context.NextRank / 2 < NumeralRankOptions.MinGap;

        return new(
            rank: needReorder
        ? NumeralRankOptions.NeedReordering
        : context.NextRank / 2);
    }

    public bool CanHandle(NumeralRankContext context)
    {
        return context.IsToTop;
    }
}

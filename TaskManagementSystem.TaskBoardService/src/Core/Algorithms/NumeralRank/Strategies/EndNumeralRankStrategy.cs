using TaskManagementSystem.SharedLib.Algorithms.NumeralRank;
using TaskManagementSystem.TaskBoardService.Core.Algorithms.NumeralRank.Interfaces;

namespace TaskManagementSystem.TaskBoardService.Core.Algorithms.NumeralRank.Strategies;


public class EndNumeralRankStrategy : INumeralRankStrategy
{

    public NumeralRankResult GenerateRank(NumeralRankContext context)
    {
        var needReorder = NumeralRankOptions.MaxRank - context.PreviousRank <= NumeralRankOptions.MinGap;

        return needReorder
            ? NumeralRankResult.ForReorder()
            : new(
                context.PreviousRank + NumeralRankOptions.DefaultStep);
    }

    public bool CanHandle(NumeralRankContext context)
    {
        return context.IsToEnd;
    }
}

using TaskManagementSystem.SharedLib.Algorithms.NumeralRank;
using NumeralRankContext = TaskManagementSystem.TaskBoardService.Core.Algorithms.NumeralRank.NumeralRankContext;

namespace TaskManagementSystem.TaskBoardService.Tests.NumeralRank;


public static class NumeralRankContextStaticTestsConstants
{
    public static NumeralRankContext FirstRankContext = new NumeralRankContext(
        previousRank: NumeralRankOptions.Empty,
        nextRank: NumeralRankOptions.Empty);

    public static NumeralRankContext TopRankContext = new NumeralRankContext(
        previousRank: NumeralRankOptions.Empty,
        nextRank: NumeralRankOptions.Default);

    public static NumeralRankContext EndRankContext = new NumeralRankContext(
        previousRank: NumeralRankOptions.Default,
        nextRank: NumeralRankOptions.Empty);

    public static NumeralRankContext BetweenRankContext = new NumeralRankContext(
        previousRank: NumeralRankOptions.Default,
        nextRank: NumeralRankOptions.Default + NumeralRankOptions.DefaultStep);

    public static NumeralRankContext TopNeedReorderingRankContext = new NumeralRankContext(
        previousRank: NumeralRankOptions.Empty,
        nextRank: NumeralRankOptions.Default + NumeralRankOptions.MinGap - 1);

    public static NumeralRankContext EndNeedReorderingRankContext = new NumeralRankContext(
        previousRank: NumeralRankOptions.MaxRank - NumeralRankOptions.MinGap + 1,
        nextRank: NumeralRankOptions.Empty);

    public static NumeralRankContext BetweenNeedReorderingRankContext = new NumeralRankContext(
        previousRank: NumeralRankOptions.Default,
        nextRank: NumeralRankOptions.Default + NumeralRankOptions.MinGap - 1);
}

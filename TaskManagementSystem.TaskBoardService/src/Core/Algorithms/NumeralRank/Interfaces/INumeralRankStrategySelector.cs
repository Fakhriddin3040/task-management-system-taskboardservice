namespace TaskManagementSystem.TaskBoardService.Core.Algorithms.NumeralRank.Interfaces;


public interface INumeralRankStrategySelector
{
    INumeralRankStrategy GetStrategy(NumeralRankContext context);
}

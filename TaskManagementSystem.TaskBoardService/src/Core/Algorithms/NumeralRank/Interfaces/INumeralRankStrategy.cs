namespace TaskManagementSystem.TaskBoardService.Core.Algorithms.NumeralRank.Interfaces;


public interface INumeralRankStrategy
{
    /// <summary>
    /// Generates a rank based on the provided contetx.
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    NumeralRankResult GenerateRank(NumeralRankContext context);

    /// <summary>
    /// Checks if the strategy can handle the provided context.
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    bool CanHandle(NumeralRankContext context);
}

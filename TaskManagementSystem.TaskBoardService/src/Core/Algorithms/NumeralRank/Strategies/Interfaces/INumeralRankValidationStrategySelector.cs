namespace TaskManagementSystem.TaskBoardService.Core.Algorithms.NumeralRank.Interfaces;


public interface INumeralRankValidationStrategySelector
{
    /// <summary>
    /// Selects a validation strategy based on the provided rank type.
    /// </summary>
    /// <param name="context"></param>
    /// <returns>An instance of INumericRankValidationStrategy.</returns>
    INumeralRankValidationStrategy GetValidationStrategy(NumeralRankContext context);
}

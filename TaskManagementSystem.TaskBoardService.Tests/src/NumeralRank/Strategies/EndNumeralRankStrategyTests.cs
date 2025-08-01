using TaskManagementSystem.SharedLib.Algorithms.NumeralRank;
using TaskManagementSystem.TaskBoardService.Core.Algorithms.NumeralRank.Interfaces;
using TaskManagementSystem.TaskBoardService.Core.Algorithms.NumeralRank.Strategies;
using NumeralRankContext = TaskManagementSystem.TaskBoardService.Core.Algorithms.NumeralRank.NumeralRankContext;

namespace TaskManagementSystem.TaskBoardService.Tests.NumeralRank.Strategies;


public class EndNumeralRankStrategyTests
{
    private readonly INumeralRankStrategy _strategy;

    public EndNumeralRankStrategyTests()
    {
        _strategy = new EndNumeralRankStrategy();
    }

    #region Can handle tests

    [Fact]
    public void CanHandle_WithValidRankContext_ReturnsTrue()
    {
        // Arrange
        var context = new NumeralRankContext(
            previousRank: NumeralRankOptions.MaxRank - NumeralRankOptions.DefaultStep,
            nextRank: NumeralRankOptions.Empty);

        // Act
        var canHandle = _strategy.CanHandle(context: context);

        // Assert
        Assert.True(canHandle, "Expected strategy to handle the context. Expected: true, Actual: false");
    }

    [Fact]
    public void IterateInvalidRankContext_EachResultIsFalse()
    {
        // Arrange
        var contexts = new[] {
            NumeralRankContextStaticTestsConstants.FirstRankContext,
            NumeralRankContextStaticTestsConstants.TopRankContext,
            NumeralRankContextStaticTestsConstants.BetweenRankContext
        };

        // Act & Assert
        foreach (var context in contexts)
        {
            var canHandle = _strategy.CanHandle(context: context);

            Assert.False(canHandle,
                $"Expected strategy to not handle the context. Expected: false, Actual: {canHandle}");
        }
    }

    #endregion

    #region Generate rank tests

    [Fact]
    public void GenerateRank_WithValidRankContext_ReturnsNextRank()
    {
        // Arrange
        var context = NumeralRankContextStaticTestsConstants.EndRankContext;
        var expectedRank = context.PreviousRank + NumeralRankOptions.DefaultStep;

        // Act
        var generatedRank = _strategy.GenerateRank(context: context);

        // Assert
        Assert.True(expectedRank == generatedRank.Rank, $"Generated wrong rank. Expected: {expectedRank}, Actual: {generatedRank.Rank}");
    }

    [Fact]
    public void GenerateRank_ValidRankContext_ReturnsNeedReordering()
    {
        // Arrange
        var context = NumeralRankContextStaticTestsConstants.EndNeedReorderingRankContext;

        // Act
        var generatedRank = _strategy.GenerateRank(context: context);

        // Assert

        Assert.True(
            generatedRank.NeedToReorder,
            "Expected generated rank to indicate reordering is needed." +
                         $"Expected: {NumeralRankOptions.NeedReordering}," +
                         $"Actual: {generatedRank.Rank}");
    }

    #endregion
}

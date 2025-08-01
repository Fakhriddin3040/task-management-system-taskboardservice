using TaskManagementSystem.SharedLib.Algorithms.NumeralRank;
using TaskManagementSystem.TaskBoardService.Core.Algorithms.NumeralRank.Interfaces;
using TaskManagementSystem.TaskBoardService.Core.Algorithms.NumeralRank.Strategies;
using NumeralRankContext = TaskManagementSystem.TaskBoardService.Core.Algorithms.NumeralRank.NumeralRankContext;

namespace TaskManagementSystem.TaskBoardService.Tests.NumeralRank.Strategies;


public class FirstNumeralRankStrategyTests
{
    private readonly INumeralRankStrategy _strategy;

    public FirstNumeralRankStrategyTests()
    {
        _strategy = new FirstNumeralRankStrategy();
    }


    /// <summary>
    /// Create a default case for first ranking of ranking context and verify that the strategy can handle it.
    /// </summary>

    #region Can handle tests

    [Fact]
    public void CallCanHandle_WithValidRankContext_ReturnsTrue()
    {
        // Arrange
        var context = new NumeralRankContext(previousRank: NumeralRankOptions.Empty, nextRank: NumeralRankOptions.Empty);

        // Act
        bool canHandle = _strategy.CanHandle(context: context);

        // Assert
        Assert.True(canHandle);
    }

    [Fact]
    public void IterateForInvalidRankContexts_ReturnsFalseForEach()
    {
        // Arrange
        var invalidContexts = new[] {
            NumeralRankContextStaticTestsConstants.BetweenRankContext,
            NumeralRankContextStaticTestsConstants.EndRankContext
        };

        // Act & Assert
        foreach (var context in invalidContexts)
        {
            var canHandle = _strategy.CanHandle(context: context);
            Assert.False(canHandle, $"Expected CanHandle to return false for context: {context}");
        }
    }

    #endregion

    #region Rank generation tests

    [Fact]
    public void GenerateValidRank_ForValidContext()
    {
        // Arrange
        var context = NumeralRankContextStaticTestsConstants.FirstRankContext;
        var expectedRank = NumeralRankOptions.Default;

        // Act
        var generatedRank = _strategy.GenerateRank(context: context);

        // Assert
        Assert.Equal(expectedRank, generatedRank.Rank);
    }

    #endregion
}

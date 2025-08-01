using TaskManagementSystem.SharedLib.Algorithms.NumeralRank;

namespace TaskManagementSystem.TaskBoardService.Core.Algorithms.NumeralRank;


public readonly struct NumeralRankContext(long previousRank, long nextRank)
{
    public long PreviousRank { get; } = previousRank;
    public long NextRank { get; } = nextRank;

    public override string ToString() => $"Previous Rank: {PreviousRank}, Next Rank: {NextRank}";

    public bool IsFirstRank => PreviousRank == NumeralRankOptions.Empty && NextRank == NumeralRankOptions.Empty;
    public bool IsToTop => PreviousRank == NumeralRankOptions.Empty && NextRank != NumeralRankOptions.Empty;
    public bool IsToEnd => PreviousRank != NumeralRankOptions.Empty && NextRank == NumeralRankOptions.Empty;
    public bool IsBetween => PreviousRank != NumeralRankOptions.Empty && NextRank != NumeralRankOptions.Empty;
}

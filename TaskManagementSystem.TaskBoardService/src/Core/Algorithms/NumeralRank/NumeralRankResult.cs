namespace TaskManagementSystem.TaskBoardService.Core.Algorithms.NumeralRank;


public readonly struct NumeralRankResult(long rank)
{
    public long Rank { get; } = rank;

    public bool IsValid => Rank > NumeralRankOptions.ValidAfter;
    public bool IsEmpty => Rank == NumeralRankOptions.Empty;
    public bool NeedToReorder => Rank == NumeralRankOptions.NeedReordering;

    public NumeralRankResult ForReorder() => new(NumeralRankOptions.NeedReordering);
    public NumeralRankResult Empty() => new(NumeralRankOptions.Empty);

    public override string ToString() => $"Rank: {Rank}";
};

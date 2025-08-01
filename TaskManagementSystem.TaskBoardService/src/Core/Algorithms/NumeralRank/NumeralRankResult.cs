using TaskManagementSystem.SharedLib.Algorithms.NumeralRank;

namespace TaskManagementSystem.TaskBoardService.Core.Algorithms.NumeralRank;


public readonly struct NumeralRankResult
{
    /// <summary>
    /// Represents the rank of a numeral.
    /// Stores flags for additional properties.
    /// </summary>
    public long Rank { get; }

    /// <summary>
    /// Bitwise flags for additional properties of the numeral rank.
    /// The bite number from right: description
    ///
    /// 1: Indicates that need to reorder.
    /// 2: Indicates that the numeral is empty.
    /// </summary>
    public ushort Flags { get; }

    public bool IsValid => !IsEmpty && !NeedToReorder;
    public bool IsEmpty => (Flags & 1 << NumeralRankOptions.EmptyShift) != 0;
    public bool NeedToReorder => (Flags & 1 << NumeralRankOptions.NeedReorderingShift) != 0;

    public static NumeralRankResult ForReorder() => new(1 << NumeralRankOptions.NeedReorderingShift);
    public static NumeralRankResult Empty() => new(1 << NumeralRankOptions.EmptyShift);

    public NumeralRankResult(long rank)
    {
        Rank = rank;
        Flags = 0;
    }

    public NumeralRankResult(ushort flags)
    {
        Flags = flags;
        Rank = -1;
    }

    public NumeralRankResult(long rank, ushort flags)
    {
        Rank = rank;
        Flags = flags;
    }

    public override string ToString() => $"Rank: {Rank}";

};

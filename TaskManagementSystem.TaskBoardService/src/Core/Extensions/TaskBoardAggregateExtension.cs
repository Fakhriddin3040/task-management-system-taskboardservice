using TaskManagementSystem.TaskBoardService.Core.Aggregates;

namespace TaskManagementSystem.TaskBoardService.Core.Extensions;


public static class TaskBoardAggregateExtension
{
    public static int GetNextColumnOrder(this TaskBoardAggregate board)
    {
        if (!board.Columns.Any())
            return 1;

        return board.Columns.Max(c => c.Order) + 1;
    }
}

using System.Linq.Expressions;
using TaskManagementSystem.TaskBoardService.Core.Algorithms.NumeralRank.Interfaces;
using TaskManagementSystem.TaskBoardService.Core.Interfaces.Repository;
using TaskManagementSystem.TaskBoardService.Core.Models;

namespace TaskManagementSystem.TaskBoardService.Core.Algorithms.NumeralRank.Strategies.Validations;


public class BetweenNumeralRankValidationStrategy : INumeralRankValidationStrategy
{
    private readonly ITaskBoardRepository _boardRepository;

    public BetweenNumeralRankValidationStrategy(ITaskBoardRepository boardRepository)
    {
        _boardRepository = boardRepository ?? throw new ArgumentNullException(nameof(boardRepository));
    }

    public async Task<bool> ValidateAsync(Guid boardId, NumeralRankContext context, CancellationToken cancellationToken)
    {
        Expression<Func<TaskBoardColumnModel, bool>> predicate = c => c.Order >= context.PreviousRank && c.Order <= context.NextRank;

        IEnumerable<TaskBoardColumnModel> columns = (await _boardRepository.FilterColumnsAsync(
            taskBoardId: boardId,
            predicate: predicate,
            cancellationToken: cancellationToken
        )).ToList();

        if (columns.Count() != 2)
        {
            return false;
        }

        var orderedColumns = columns.OrderBy(c => c.Order).ToList();

        if (orderedColumns[0].Order != context.PreviousRank ||
            orderedColumns[1].Order != context.NextRank ||
            orderedColumns[0].Order > orderedColumns[1].Order ||
            orderedColumns[0].Order == orderedColumns[1].Order)
        {
            return false;
        }

        return true;
    }
    public bool CanHandle(NumeralRankContext rankContext)
    {
        return rankContext.IsBetween;
    }
}

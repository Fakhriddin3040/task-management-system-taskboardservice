using System.Linq.Expressions;
using TaskManagementSystem.TaskBoardService.Core.Algorithms.NumeralRank.Interfaces;
using TaskManagementSystem.TaskBoardService.Core.Interfaces.Repository;
using TaskManagementSystem.TaskBoardService.Core.Models;

namespace TaskManagementSystem.TaskBoardService.Core.Algorithms.NumeralRank.Strategies.Validations;


public class TopNumeralRankValidationStrategy : INumeralRankValidationStrategy
{
    private readonly ITaskBoardRepository _boardRepository;

    public TopNumeralRankValidationStrategy(ITaskBoardRepository boardRepository)
    {
        _boardRepository = boardRepository;
    }

    public async Task<bool> ValidateAsync(Guid boardId, NumeralRankContext context, CancellationToken cancellationToken)
    {
        Expression<Func<TaskBoardColumnModel, bool>> filter = column => column.Order <= context.NextRank;

        var columns = (await _boardRepository.FilterColumnsAsync(
            taskBoardId: boardId,
            predicate: filter,
            cancellationToken: cancellationToken
        )).ToList();

        return columns.Count == 1;
    }
    public bool CanHandle(NumeralRankContext rankContext)
    {
        return rankContext.IsToTop;
    }
}

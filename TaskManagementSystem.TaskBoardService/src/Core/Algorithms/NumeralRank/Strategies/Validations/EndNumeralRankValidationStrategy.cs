using System.Linq.Expressions;
using TaskManagementSystem.TaskBoardService.Core.Algorithms.NumeralRank.Interfaces;
using TaskManagementSystem.TaskBoardService.Core.Interfaces.Repository;
using TaskManagementSystem.TaskBoardService.Core.Models;

namespace TaskManagementSystem.TaskBoardService.Core.Algorithms.NumeralRank.Strategies.Validations;


public class EndNumeralRankValidationStrategy : INumeralRankValidationStrategy
{
    private readonly ITaskBoardRepository _boardRepository;

    public EndNumeralRankValidationStrategy(ITaskBoardRepository boardRepository)
    {
        _boardRepository = boardRepository;
    }

    public async Task<bool> ValidateAsync(Guid boardId, NumeralRankContext context, CancellationToken cancellationToken)
    {
        Expression<Func<TaskBoardColumnModel, bool>> filter = col => col.Order >= context.PreviousRank;

        var columns = (await _boardRepository.FilterColumnsAsync(
            taskBoardId: boardId,
            predicate: filter,
            cancellationToken: cancellationToken
        )).ToList();

        return columns.Count == 1;
    }

    public bool CanHandle(NumeralRankContext rankContext)
    {
        return rankContext.IsToEnd;
    }
}

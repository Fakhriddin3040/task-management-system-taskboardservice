using MediatR;
using TaskManagementSystem.SharedLib.Abstractions.Interfaces;
using TaskManagementSystem.SharedLib.Enums.Exceptions;
using TaskManagementSystem.SharedLib.Exceptions;
using TaskManagementSystem.TaskBoardService.Core.Algorithms.NumeralRank;
using TaskManagementSystem.TaskBoardService.Core.Algorithms.NumeralRank.Interfaces;
using TaskManagementSystem.TaskBoardService.Core.Interfaces.Repository;
using TaskManagementSystem.TaskBoardService.Core.Models;
using ExecutionContext = TaskManagementSystem.SharedLib.DTO.ExecutionContext;

namespace TaskManagementSystem.TaskBoardService.Application.Commands.Handlers;


public class MoveColumnCommandHandler : IRequestHandler<MoveColumnCommand>
{
    private readonly ITaskBoardRepository _boardRepository;
    private readonly INumeralRankStrategySelector _rankStrategySelector;
    private readonly INumeralRankValidationStrategySelector _rankValidationStrategySelector;
    private readonly IDateTimeService _dateTimeService;
    private readonly ExecutionContext _context;

    public async Task Handle(MoveColumnCommand request, CancellationToken cancellationToken)
    {
        var board = await _boardRepository.GetByColumnIdAsync(request.ColumnId, cancellationToken);

        if (board is null)
        {
            throw new AppException(
                statusCode: AppExceptionStatusCode.NotFound,
                message: AppExceptionErrorMessages.NotFound
            );
        }

        var rankingContext = GetRankingContext(columns: board.Columns, request: request);
        await Validate(context: rankingContext);

        var result = board.MoveColumn(
            columnId: request.ColumnId,
            rankContext: rankingContext,
            updatedById: _context.User.Id,
            dateTimeService: _dateTimeService,
            rankStrategySelector: _rankStrategySelector
        );

        if (result.IsValid)
        {
            return;
        }

        if (result.NeedToReorder)
        {
            throw new NotImplementedException("Need to implement reordering logic.");
        }

        throw new AppUnexpectedException();
    }

    private async Task Validate(NumeralRankContext context)
    {
        var validationStrategy = _rankValidationStrategySelector.GetValidationStrategy(context: context);

        if (!await validationStrategy.ValidateAsync(context))
        {
            throw new AppException(
                statusCode: AppExceptionStatusCode.BadRequest,
                message: AppExceptionErrorMessages.InvalidColumnRangeOnMove
            );
        }

    }

    private NumeralRankContext GetRankingContext(
        IEnumerable<TaskBoardColumnModel> columns,
        MoveColumnCommand request)
    {
        var filtered = columns.Where(
            c => c.Id == request.PreviousId || c.Id == request.NextId
        );
        return new NumeralRankContext(
            previousRank: filtered.FirstOrDefault(c => c.Id == request.PreviousId)?.Order ?? NumeralRankOptions.Empty,
            nextRank: filtered.FirstOrDefault(c => c.Id == request.NextId)?.Order ?? NumeralRankOptions.Empty
        );
    }

    // private NumeralRankContext GetRankingContext(
    //     IEnumerable<TaskBoardColumnModel> columns,
    //     MoveColumnCommand request
    //     )
    // {
    //     var ordered = columns.OrderBy(c => c.Order).ToList();
    //
    //     var targetIdx = ordered.FindIndex(c => c.Id == request.ColumnId);
    //
    //     if (targetIdx == -1)
    //     {
    //         throw new InvalidProgramException(
    //             $"Column with ID {request.ColumnId} not found in the provided columns, but was checked in handler."
    //         );
    //     }
    //
    //     var prev = targetIdx > 0 ? ordered[targetIdx - 1] : null;
    //     var next = targetIdx < ordered.Count - 1 ? ordered[targetIdx + 1] : null;
    //
    //     if (prev is not null && next is not null)
    //     {
    //         var anyBetween = ordered.Any(c => c.Order > prev.Order && c.Order < next.Order);
    //
    //         if (anyBetween)
    //         {
    //             throw new AppException(
    //                 statusCode: AppExceptionStatusCode.BadRequest,
    //                 message: "Cannot move column to a position that is not empty."
    //             );
    //         }
    //     }
    //
    //     return new NumeralRankContext(
    //         previousRank: prev?.Order ?? NumeralRankOptions.Empty,
    //         nextRank: next?.Order ?? NumeralRankOptions.Empty
    //     );
    // }
}

using MediatR;
using TaskManagementSystem.SharedLib.Abstractions.Interfaces;
using TaskManagementSystem.SharedLib.Algorithms.NumeralRank;
using TaskManagementSystem.SharedLib.Enums.Exceptions;
using TaskManagementSystem.SharedLib.Exceptions;
using TaskManagementSystem.SharedLib.Providers.Interfaces;
using TaskManagementSystem.TaskBoardService.Core.Algorithms.NumeralRank.Interfaces;
using TaskManagementSystem.TaskBoardService.Core.Interfaces.Repository;
using TaskManagementSystem.TaskBoardService.Core.Models;
using ExecutionContext = TaskManagementSystem.SharedLib.DTO.ExecutionContext;
using NumeralRankContext = TaskManagementSystem.TaskBoardService.Core.Algorithms.NumeralRank.NumeralRankContext;

namespace TaskManagementSystem.TaskBoardService.Application.Commands.Handlers;


public class MoveColumnCommandHandler : IRequestHandler<MoveColumnCommand>
{
    private readonly ITaskBoardRepository _boardRepository;
    private readonly INumeralRankStrategySelector _rankStrategySelector;
    private readonly INumeralRankValidationStrategySelector _rankValidationStrategySelector;
    private readonly IDateTimeService _dateTimeService;
    private readonly ExecutionContext _context;

    public MoveColumnCommandHandler(
        ITaskBoardRepository boardRepository,
        INumeralRankStrategySelector rankStrategySelector,
        INumeralRankValidationStrategySelector rankValidationStrategySelector,
        IDateTimeService dateTimeService,
        IExecutionContextProvider contextProvider)
    {
        _boardRepository = boardRepository;
        _rankStrategySelector = rankStrategySelector;
        _rankValidationStrategySelector = rankValidationStrategySelector;
        _dateTimeService = dateTimeService;
        _context = contextProvider.GetContext();
    }

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
        Console.WriteLine(rankingContext);
        await Validate(
            boardId: board.Id,
            rankingContext: rankingContext,
            cancellationToken: cancellationToken
        );

        var result = board.MoveColumn(
            columnId: request.ColumnId,
            rankContext: rankingContext,
            updatedById: _context.User.Id,
            dateTimeService: _dateTimeService,
            rankStrategySelector: _rankStrategySelector
        );

        if (result.IsValid)
        {
            Console.WriteLine("Saving...");
            await _boardRepository.SaveChangesAsync(cancellationToken);
            return;
        }

        if (result.NeedToReorder)
        {
            throw new NotImplementedException("Need to implement reordering logic.");
        }

        throw new AppUnexpectedException();
    }

    private async Task Validate(Guid boardId, NumeralRankContext rankingContext, CancellationToken cancellationToken)
    {
        var validationStrategy = _rankValidationStrategySelector.GetValidationStrategy(context: rankingContext);

        Console.WriteLine(validationStrategy.GetType());

        if (!await validationStrategy.ValidateAsync(
                boardId: boardId,
                context: rankingContext,
                cancellationToken))
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
        ).ToList();

        return new NumeralRankContext(
            previousRank: filtered.FirstOrDefault(c => c.Id == request.PreviousId)?.Order ?? NumeralRankOptions.Empty,
            nextRank: filtered.FirstOrDefault(c => c.Id == request.NextId)?.Order ?? NumeralRankOptions.Empty
        );
    }
}

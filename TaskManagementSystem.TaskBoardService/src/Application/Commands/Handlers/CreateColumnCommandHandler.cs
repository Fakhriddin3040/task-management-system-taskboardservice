using MediatR;
using TaskManagementSystem.SharedLib.Abstractions.Interfaces;
using TaskManagementSystem.SharedLib.Exceptions;
using TaskManagementSystem.SharedLib.Handlers;
using TaskManagementSystem.SharedLib.Providers.Interfaces;
using TaskManagementSystem.TaskBoardService.Application.Commands.Results;
using TaskManagementSystem.TaskBoardService.Core.Algorithms.NumeralRank.Interfaces;
using TaskManagementSystem.TaskBoardService.Core.Algorithms.NumeralRank.Strategies;
using TaskManagementSystem.TaskBoardService.Core.Interfaces.Policies;
using TaskManagementSystem.TaskBoardService.Core.Interfaces.Repository;
using ExecutionContext = TaskManagementSystem.SharedLib.DTO.ExecutionContext;

namespace TaskManagementSystem.TaskBoardService.Application.Commands.Handlers;


public class CreateColumnCommandHandler : IRequestHandler<CreateColumnCommand, Result<CreateColumnCommandResult>>
{
    private readonly ITaskBoardRepository _boardRepository;
    private readonly IValidColumnNamePolicy _validColumnNamePolicy;
    private readonly IUniqueColumnNamePolicy _uniqueColumnNamePolicy;
    private readonly IDateTimeService _dateTimeService;
    private readonly ExecutionContext _context;
    private readonly INumeralRankStrategySelector _rankStrategySelector;

    public CreateColumnCommandHandler(
        ITaskBoardRepository boardRepository,
        IExecutionContextProvider contextProvider,
        IValidColumnNamePolicy validColumnNamePolicy,
        IUniqueColumnNamePolicy uniqueColumnNamePolicy,
        IDateTimeService dateTimeService
        )
    {
        _boardRepository = boardRepository;
        _context = contextProvider.GetContext();
        _validColumnNamePolicy = validColumnNamePolicy;
        _uniqueColumnNamePolicy = uniqueColumnNamePolicy;
        _dateTimeService = dateTimeService;
        _rankStrategySelector = new NumeralRankStrategySelector(
            [
                new EndNumeralRankStrategy(),
                new FirstNumeralRankStrategy()
            ]
        );
    }

    public async Task<Result<CreateColumnCommandResult>> Handle(CreateColumnCommand request, CancellationToken cancellationToken)
    {
        var board = await _boardRepository.GetByIdAsync(request.BoardId, cancellationToken);

        if (board == null)
        {
            throw AppException.NotFound();
        }

        var result = await board.AddColumnAsync(
            name: request.Name,
            createdById: _context.User.Id,
            cancellationToken: cancellationToken,
            numeralRankStrategySelector: _rankStrategySelector,
            dateTimeService: _dateTimeService,
            namePolicy: _validColumnNamePolicy,
            uniqueNamePolicy: _uniqueColumnNamePolicy
        );

        if (result.IsFailure)
        {
            return Result<CreateColumnCommandResult>.Failure(result.ErrorDetails);
        }

        if (result.Value.NeedToReorder)
        {
            throw new NotImplementedException("Not implemented yet!");
        }
        if (!result.Value.IsValid)
        {
            throw new AppUnexpectedException();
        }

        _boardRepository.Update(board);
        await _boardRepository.SaveChangesAsync(cancellationToken);

        var column = board.Columns.First(c => c.Order == result.Value.Rank);

        return Result<CreateColumnCommandResult>.Success(
            new(
                id: column.Id,
                order: column.Order
            )
        );
    }
}

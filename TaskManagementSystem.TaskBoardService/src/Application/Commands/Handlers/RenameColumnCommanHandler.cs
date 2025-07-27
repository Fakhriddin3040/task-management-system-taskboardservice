using MediatR;
using TaskManagementSystem.SharedLib.Abstractions.Interfaces;
using TaskManagementSystem.SharedLib.Enums.Exceptions;
using TaskManagementSystem.SharedLib.Exceptions;
using TaskManagementSystem.SharedLib.Extensions;
using TaskManagementSystem.SharedLib.Handlers;
using TaskManagementSystem.SharedLib.Providers.Interfaces;
using TaskManagementSystem.TaskBoardService.Core.Aggregates;
using TaskManagementSystem.TaskBoardService.Core.Interfaces.Policies;
using TaskManagementSystem.TaskBoardService.Core.Interfaces.Repository;
using ExecutionContext = TaskManagementSystem.SharedLib.DTO.ExecutionContext;
using Unit = TaskManagementSystem.SharedLib.Structs.Unit;

namespace TaskManagementSystem.TaskBoardService.Application.Commands.Handlers;


public class UpdateColumnCommandHandler : IRequestHandler<RenameColumnCommand, Result<Unit>>
{
    private readonly ITaskBoardRepository _boardRepository;
    private readonly IDateTimeService _dateTimeService;
    private readonly IValidColumnNamePolicy _namePolicy;
    private readonly IUniqueColumnNamePolicy _uniqueNamePolicy;
    private readonly ExecutionContext _context;

    public UpdateColumnCommandHandler(
        ITaskBoardRepository boardRepository,
        IDateTimeService dateTimeService,
        IValidColumnNamePolicy namePolicy,
        IUniqueColumnNamePolicy uniqueNamePolicy,
        IExecutionContextProvider contextProvider)
    {
        _boardRepository = boardRepository;
        _dateTimeService = dateTimeService;
        _namePolicy = namePolicy;
        _uniqueNamePolicy = uniqueNamePolicy;
        _context = contextProvider.GetContext();
    }

    public async Task<Result<Unit>> Handle(RenameColumnCommand request, CancellationToken cancellationToken)
    {
        var board = await _boardRepository.GetByColumnIdAsync(request.ColumnId, cancellationToken);
        if (board == null)
            throw new AppException(
                message: AppExceptionErrorMessages.NotFound,
                statusCode: AppExceptionStatusCode.NotFound
            );

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            var localResult = await board.RenameColumnAsync(
                columnId: request.ColumnId,
                newName: request.Name,
                cancellationToken: cancellationToken,
                dateTimeService: _dateTimeService,
                updatedById: _context.User.Id,
                namePolicy: _namePolicy,
                uniqueNamePolicy: _uniqueNamePolicy
            );

            if (localResult.IsFailure)
            {
                errors = errors.Merge(localResult);
            }
        }

        MoveColumns()

        _boardRepository.Update(board);

        await _boardRepository.SaveChangesAsync(cancellationToken);

        return Result<Unit>.Success(Unit.Value);
    }
}

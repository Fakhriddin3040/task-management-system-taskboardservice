using MediatR;
using TaskManagementSystem.SharedLib.Abstractions.Interfaces;
using TaskManagementSystem.SharedLib.Enums.Exceptions;
using TaskManagementSystem.SharedLib.Exceptions;
using TaskManagementSystem.SharedLib.Handlers;
using TaskManagementSystem.SharedLib.Providers.Interfaces;
using TaskManagementSystem.TaskBoardService.Core.Interfaces.Policies;
using TaskManagementSystem.TaskBoardService.Core.Interfaces.Repository;
using ExecutionContext = TaskManagementSystem.SharedLib.DTO.ExecutionContext;
using Unit = TaskManagementSystem.SharedLib.Structs.Unit;

namespace TaskManagementSystem.TaskBoardService.Application.Commands.Handlers;


public class UpdateColumnCommandHandler : IRequestHandler<UpdateColumnCommand, Result<Unit>>
{
    private readonly ITaskBoardRepository _boardRepository;
    private readonly IDateTimeService _dateTimeService;
    private readonly IValidColumnNamePolicy _namePolicy;
    private readonly IUniqueColumnNamePolicy _uniqueNamePolicy;
    private readonly ExecutionContext _executionContext;

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
        _executionContext = contextProvider.GetContext();
    }

    public async Task<Result<Unit>> Handle(UpdateColumnCommand request, CancellationToken cancellationToken)
    {
        var board = await _boardRepository.GetByColumnIdAsync(request.ColumnId, cancellationToken);

        if (board == null)
            throw new AppException(
                message: AppExceptionErrorMessages.NotFound,
                statusCode: AppExceptionStatusCode.NotFound
            );

        var result = await board.UpdateColumnAsync(
            columnId: request.ColumnId,
            updatedById: _executionContext.User.Id,
            name: request.Name,
            order: request.Order,
            cancellationToken: cancellationToken,
            namePolicy: _namePolicy,
            uniqueNamePolicy: _uniqueNamePolicy,
            dateTimeService: _dateTimeService
        );

        if (result.IsFailure)
        {
            return Result<Unit>.Failure(result.ErrorDetails);
        }

        _boardRepository.Update(board);

        await _boardRepository.SaveChangesAsync(cancellationToken);

        return Result<Unit>.Success(Unit.Value);
    }
}

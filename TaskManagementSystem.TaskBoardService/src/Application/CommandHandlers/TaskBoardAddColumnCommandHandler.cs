using MediatR;
using TaskManagementSystem.SharedLib.Abstractions.Interfaces;
using TaskManagementSystem.SharedLib.Enums.Exceptions;
using TaskManagementSystem.SharedLib.Exceptions;
using TaskManagementSystem.SharedLib.Handlers;
using TaskManagementSystem.SharedLib.Providers.Interfaces;
using TaskManagementSystem.TaskBoardService.Application.Commands;
using TaskManagementSystem.TaskBoardService.Application.Results;
using TaskManagementSystem.TaskBoardService.Core.Interfaces.Policies;
using TaskManagementSystem.TaskBoardService.Core.Interfaces.Repository;
using ExecutionContext = TaskManagementSystem.SharedLib.DTO.ExecutionContext;

namespace TaskManagementSystem.TaskBoardService.Application.CommandHandlers;


public class TaskBoardAddColumnCommandHandler : IRequestHandler<TaskBoardAddColumnCommand, Result<TaskBoardAddColumnResult>>
{
    private readonly ITaskBoardRepository _boardRepository;
    private readonly IValidColumnNamePolicy _validColumnNamePolicy;
    private readonly IUniqueTaskBoardColumnNamePolicy _uniqueColumnNamePolicy;
    private readonly IDateTimeService _dateTimeService;
    private readonly ExecutionContext _context;

    public TaskBoardAddColumnCommandHandler(
        ITaskBoardRepository boardRepository,
        IExecutionContextProvider contextProvider,
        IValidColumnNamePolicy validColumnNamePolicy,
        IUniqueTaskBoardColumnNamePolicy uniqueColumnNamePolicy,
        IDateTimeService dateTimeService
        )
    {
        _boardRepository = boardRepository;
        _context = contextProvider.GetContext();
        _validColumnNamePolicy = validColumnNamePolicy;
        _uniqueColumnNamePolicy = uniqueColumnNamePolicy;
        _dateTimeService = dateTimeService;
    }

    public async Task<Result<TaskBoardAddColumnResult>> Handle(TaskBoardAddColumnCommand request, CancellationToken cancellationToken)
    {
        var board = await _boardRepository.GetByIdAsync(request.BoardId, cancellationToken);

        if (board == null)
        {
            throw new AppException(
                message: AppExceptionErrorMessages.NotFound,
                statusCode: AppExceptionStatusCode.NotFound
            );
        }

        var result = await board.AddColumn(
            name: request.Name,
            createdById: _context.User.Id,
            cancellationToken: cancellationToken,
            dateTimeService: _dateTimeService,
            namePolicy: _validColumnNamePolicy,
            uniqueNamePolicy: _uniqueColumnNamePolicy
        );

        if (result.IsFailure)
        {
            return Result<TaskBoardAddColumnResult>.Failure(result.ErrorDetails);
        }
        await _boardRepository.UpdateAsync(board, cancellationToken);
        await _boardRepository.SaveChangesAsync(cancellationToken);

        return Result<TaskBoardAddColumnResult>.Success(
            new(
                id: result.Value.Id,
                order: result.Value.Order
            )
        );
    }
}

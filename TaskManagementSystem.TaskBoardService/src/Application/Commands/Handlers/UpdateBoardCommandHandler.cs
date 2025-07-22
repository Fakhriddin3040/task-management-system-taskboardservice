using MediatR;
using TaskManagementSystem.SharedLib.Abstractions.Interfaces;
using TaskManagementSystem.SharedLib.Exceptions;
using TaskManagementSystem.SharedLib.Handlers;
using TaskManagementSystem.SharedLib.Providers.Interfaces;
using TaskManagementSystem.TaskBoardService.Application.Commands.Results;
using TaskManagementSystem.TaskBoardService.Core.Interfaces.Policies;
using TaskManagementSystem.TaskBoardService.Core.Interfaces.Repository;
using ExecutionContext = TaskManagementSystem.SharedLib.DTO.ExecutionContext;

namespace TaskManagementSystem.TaskBoardService.Application.Commands.Handlers;


public sealed class UpdateBoardCommandHandler : IRequestHandler<UpdateBoardCommand, Result<UpdateBoardCommandResult>>
{
    private readonly ITaskBoardRepository _repository;
    private readonly IDateTimeService _dateTimeService;
    private readonly ExecutionContext _context;
    private readonly IValidBoardNamePolicy _validBoardNamePolicy;
    private readonly IUniqueBoardNamePolicy _uniqueBoardNamePolicy;

    public UpdateBoardCommandHandler(
        ITaskBoardRepository repository,
        IDateTimeService dateTimeService,
        IExecutionContextProvider contextProvider,
        IValidBoardNamePolicy validBoardNamePolicy,
        IUniqueBoardNamePolicy uniqueBoardNamePolicy)
    {
        _repository = repository;
        _dateTimeService = dateTimeService;
        _context = contextProvider.GetContext();
        _validBoardNamePolicy = validBoardNamePolicy;
        _uniqueBoardNamePolicy = uniqueBoardNamePolicy;
    }

    public async Task<Result<UpdateBoardCommandResult>> Handle(UpdateBoardCommand request, CancellationToken cancellationToken)
    {
        var board = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (board is null)
        {
            throw AppException.NotFound();
        }

        var result = await board.UpdateAsync(
            name: request.Name,
            description: request.Description,
            dateTimeService: _dateTimeService,
            updateById: _context.User.Id,
            namePolicy: _validBoardNamePolicy,
            uniqueNamePolicy: _uniqueBoardNamePolicy,
            cancellationToken: cancellationToken
        );

        if (result.IsFailure)
        {
            return Result<UpdateBoardCommandResult>.Failure(result.ErrorDetails);
        }

        _repository.Update(board);

        var updateResult = await _repository.SaveChangesAsync(cancellationToken);

        if (updateResult == 0)
        {
            throw AppException.InternalServerError();
        }

        return Result<UpdateBoardCommandResult>.Success(UpdateBoardCommandResult.Value);
    }
}

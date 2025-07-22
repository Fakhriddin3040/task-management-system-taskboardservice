using MediatR;
using TaskManagementSystem.SharedLib.Abstractions.Interfaces;
using TaskManagementSystem.SharedLib.DTO;
using TaskManagementSystem.SharedLib.Handlers;
using TaskManagementSystem.SharedLib.Providers.Interfaces;
using TaskManagementSystem.TaskBoardService.Application.Commands;
using TaskManagementSystem.TaskBoardService.Application.Commands.Results;
using TaskManagementSystem.TaskBoardService.Core.Aggregates;
using TaskManagementSystem.TaskBoardService.Core.Interfaces.Policies;
using TaskManagementSystem.TaskBoardService.Core.Interfaces.Repository;
using ExecutionContext = TaskManagementSystem.SharedLib.DTO.ExecutionContext;

namespace TaskManagementSystem.TaskBoardService.Application.Commands.Handlers;


public class CreateBoardCommandHandler : IRequestHandler<CreateBoardCommand, Result<CreateBoardCommandResult>>
{
    private readonly ExecutionContext _executionContext;
    private readonly ITaskBoardRepository _boardRepository;
    private readonly IDateTimeService _dateTimeService;
    private readonly IUniqueBoardNamePolicy _uniqueNamePolicy;
    private readonly IValidBoardNamePolicy _namePolicy;
    private readonly ILogger<CreateBoardCommandHandler> _logger;

    public CreateBoardCommandHandler(
        IExecutionContextProvider executionContextProvider,
        IDateTimeService dateTimeService,
        IUniqueBoardNamePolicy uniqueNamePolicy,
        ITaskBoardRepository taskBoardRepository,
        IValidBoardNamePolicy namePolicy,
        ILogger<CreateBoardCommandHandler> logger)
    {
        _executionContext = executionContextProvider.GetContext();
        _dateTimeService = dateTimeService;
        _boardRepository = taskBoardRepository;
        _uniqueNamePolicy = uniqueNamePolicy;
        _namePolicy = namePolicy;
        _logger = logger;
    }

    public async Task<Result<CreateBoardCommandResult>> Handle(CreateBoardCommand request, CancellationToken cancellationToken)
    {
        var result = await TaskBoardAggregate.CreateAsync(
            name: request.Name,
            description: request.Description,
            organizationId: Guid.NewGuid(),
            createdById: _executionContext.User.Id,
            cancellationToken: cancellationToken,
            dateTimeService: _dateTimeService,
            uniqueNamePolicy: _uniqueNamePolicy,
            namePolicy: _namePolicy
        );

        if (result.IsFailure)
        {
            return Result<CreateBoardCommandResult>.Failure(result.ErrorDetails);
        }

        await _boardRepository.CreateAsync(result.Value, cancellationToken);
        await _boardRepository.SaveChangesAsync(cancellationToken);

        return Result<CreateBoardCommandResult>.Success(
            new (result.Value.Id)
        );
    }
}

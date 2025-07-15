using MediatR;
using TaskManagementSystem.SharedLib.Abstractions.Interfaces;
using TaskManagementSystem.SharedLib.DTO;
using TaskManagementSystem.SharedLib.Handlers;
using TaskManagementSystem.SharedLib.Providers.Interfaces;
using TaskManagementSystem.TaskBoardService.Application.Commands;
using TaskManagementSystem.TaskBoardService.Application.Results;
using TaskManagementSystem.TaskBoardService.Core.Aggregates;
using TaskManagementSystem.TaskBoardService.Core.Interfaces.Policies;
using TaskManagementSystem.TaskBoardService.Core.Interfaces.Repository;
using ExecutionContext = TaskManagementSystem.SharedLib.DTO.ExecutionContext;

namespace TaskManagementSystem.TaskBoardService.Application.CommandHandlers;


public class TaskBoardCreateCommandHandler : IRequestHandler<TaskBoardCreateCommand, Result<TaskBoardCreateResult>>
{
    private readonly ExecutionContext _executionContext;
    private readonly ITaskBoardRepository _boardRepository;
    private readonly IDateTimeService _dateTimeService;
    private readonly IUniqueTaskBoardNamePolicy _uniqueNamePolicy;
    private readonly IValidBoardNamePolicy _namePolicy;
    private readonly ILogger<TaskBoardCreateCommandHandler> _logger;

    public TaskBoardCreateCommandHandler(
        IExecutionContextProvider executionContextProvider,
        IDateTimeService dateTimeService,
        IUniqueTaskBoardNamePolicy uniqueNamePolicy,
        ITaskBoardRepository taskBoardRepository,
        IValidBoardNamePolicy namePolicy,
        ILogger<TaskBoardCreateCommandHandler> logger)
    {
        _executionContext = executionContextProvider.GetContext();
        _dateTimeService = dateTimeService;
        _boardRepository = taskBoardRepository;
        _uniqueNamePolicy = uniqueNamePolicy;
        _namePolicy = namePolicy;
        _logger = logger;
    }

    public async Task<Result<TaskBoardCreateResult>> Handle(TaskBoardCreateCommand request, CancellationToken cancellationToken)
    {
        var result = await TaskBoardAggregate.Create(
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
            return Result<TaskBoardCreateResult>.Failure(result.ErrorDetails);
        }

        await _boardRepository.CreateAsync(result.Value, cancellationToken);
        await _boardRepository.SaveChangesAsync(cancellationToken);

        return Result<TaskBoardCreateResult>.Success(
            new (result.Value.Id)
        );
    }
}

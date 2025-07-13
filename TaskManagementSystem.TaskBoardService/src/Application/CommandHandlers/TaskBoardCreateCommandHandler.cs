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

namespace TaskManagementSystem.TaskBoardService.Application.CommandHandlers;


public class TaskBoardCreateCommandHandler : IRequestHandler<TaskBoardCreateCommand, Result<TaskBoardCreateResult>>
{
    private readonly ExecutionContextDto _executionContext;
    private readonly ITaskBoardRepository _boardRepository;
    private readonly IDateTimeService _dateTimeService;
    private readonly IUniqueTaskBoardNamePolicy _uniqueNamePolicy;
    private readonly IValidNamePolicy _namePolicy;

    public TaskBoardCreateCommandHandler(
        IExecutionContextProvider executionContextProvider,
        IDateTimeService dateTimeService,
        IUniqueTaskBoardNamePolicy uniqueNamePolicy,
        ITaskBoardRepository taskBoardRepository,
        IValidNamePolicy namePolicy)
    {
        _executionContext = executionContextProvider.GetContext();
        _dateTimeService = dateTimeService;
        _boardRepository = taskBoardRepository;
        _uniqueNamePolicy = uniqueNamePolicy;
        _namePolicy = namePolicy;
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

        return Result<TaskBoardCreateResult>.Success(
            new (result.Value.Id)
        );
    }
}

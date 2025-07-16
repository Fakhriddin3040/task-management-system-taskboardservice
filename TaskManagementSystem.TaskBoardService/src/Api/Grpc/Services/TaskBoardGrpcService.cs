

using Grpc.Core;
using MediatR;
using TaskManagementSystem.GrpcLib.TaskBoardService.Types;
using TaskManagementSystem.SharedLib.Exceptions;
using TaskManagementSystem.SharedLib.Extensions;
using TaskManagementSystem.TaskBoardService.Application.Commands;
using TaskManagementSystem.TaskBoardService.Core.Models;

namespace TaskManagementSystem.TaskBoardService.Api.Grpc.Services;


public class TaskBoardGrpcService : GrpcLib.TaskBoardService.Services.TaskBoardService.TaskBoardServiceBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<TaskBoardGrpcService> _logger;

    public TaskBoardGrpcService(IMediator mediator, ILogger<TaskBoardGrpcService> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public override async Task<TaskBoardCreateResponse> Create(TaskBoardCreateRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Creating task board with name: {Name}", request.Name);

        var command = new TaskBoardCreateCommand(
            name: request.Name,
            description: request.Description
        );
        var result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            _logger.LogError("Failed to create task board. Details in str format: {ErrorDetails}", result.ErrorDetailsToJson());
            result.GetValueOrThrow();
        }
        _logger.LogInformation("Created task board with ID: {TaskBoardId}", result.Value.TaskBoardId);

        return new TaskBoardCreateResponse {
            Id = result.Value.TaskBoardId.ToString()
        };
    }

    public override async Task<TaskBoardColumnCreateResponse> CreateColumn(TaskBoardColumnCreateRequest request, ServerCallContext context)
    {
        _logger.LogInformation(
            "Creating task board column with name: {Name} for board ID: {BoardId}", request.Name, request.TaskBoardId
            );
        var command = new TaskBoardAddColumnCommand(
            boardId: Guid.Parse(request.TaskBoardId),
            name: request.Name
        );

        var result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            _logger.LogError("Failed to create task board column. Details in json format: {ErrorDetails}", result.ErrorDetailsToJson());
            throw result.CreateExceptionFrom();
        }

        return new TaskBoardColumnCreateResponse {
            Id = result.Value.Id.ToString(),
            Order = result.Value.Order
        };
    }
}

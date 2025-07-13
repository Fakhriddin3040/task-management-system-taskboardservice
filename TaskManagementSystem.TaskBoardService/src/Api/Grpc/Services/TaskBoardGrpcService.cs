

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

    public TaskBoardGrpcService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<TaskBoardCreateResponse> Create(TaskBoardCreateRequest request, ServerCallContext context)
    {
        var command = new TaskBoardCreateCommand(
            name: request.Name,
            description: request.Description
        );
        var result = await _mediator.Send(command);

        return new TaskBoardCreateResponse {
            Id = result.GetValueOrThrow().TaskBoardId.ToString()
        };
    }
}

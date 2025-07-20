using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using TaskManagementSystem.GrpcLib.TaskBoardService.Types;
using TaskManagementSystem.SharedLib.Extensions;
using TaskManagementSystem.TaskBoardService.Application.Commands;
using TaskManagementSystem.TaskBoardService.Application.Queries;

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

    public override async Task<TaskBoardCreateProtoResponse> Create(TaskBoardCreateProtoRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Creating task board with name: {Name}", request.Name);

        var command = new CreateBoardCommand(
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

        return new TaskBoardCreateProtoResponse {
            Id = result.Value.TaskBoardId.ToString()
        };
    }

    public override async Task<TaskBoardColumnCreateProtoResponse> CreateColumn(TaskBoardColumnCreateProtoRequest request, ServerCallContext context)
    {
        _logger.LogInformation(
            "Creating task board column with name: {Name} for board ID: {BoardId}", request.Name, request.TaskBoardId
            );
        var command = new CreateColumnCommand(
            boardId: Guid.Parse(request.TaskBoardId),
            name: request.Name
        );

        var result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            _logger.LogError("Failed to create task board column. Details in json format: {ErrorDetails}", result.ErrorDetailsToJson());
            throw result.CreateExceptionFrom();
        }

        return new TaskBoardColumnCreateProtoResponse {
            Id = result.Value.Id.ToString(),
            Order = result.Value.Order
        };
    }

    public override async Task<Empty> UpdateColumn(TaskBoardColumnUpdateProtoRequest request, ServerCallContext context)
    {
        var command = new UpdateColumnCommand(
            columnId: Guid.Parse(request.Id),
            name: request.Name,
            order: request.Order
        );

        var result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            _logger.LogError("Failed to update task board column. Details in json format: {ErrorDetails}", result.ErrorDetailsToJson());
            throw result.CreateExceptionFrom();
        }

        _logger.LogInformation("Updated task board column with ID: {ColumnId}", request.Id);

        return new Empty();
    }

    public override async Task<TaskBoardDetailedProto> GetById(TaskBoardGetProtoRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Handling 'Get board by id'. Board id: {}", request.Id);
        var command = new GetBoardByIdQuery(Id: Guid.Parse(request.Id));

        var result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            _logger.LogError("Failed to complete the query. Details in json format: {ErrorDetails}.", result.ErrorDetailsToJson());
        }

        var response = new TaskBoardDetailedProto {
            Id = result.Value.Id.ToString(),
            Name = result.Value.Name,
            Description = result.Value.Description,
            CreatedById = result.Value.AuthorInfo.CreatedById.ToString(),
            UpdatedById = result.Value.AuthorInfo.UpdatedById.ToString(),
            CreatedAt = result.Value.Timestamps.CreatedAt,
            UpdatedAt = result.Value.Timestamps.UpdatedAt
        };

        response.Columns.AddRange(
            result.Value.Columns.Select(col => new TaskBoardColumnProto {
                Id = col.Id.ToString(),
                Name = col.Name,
                Order = col.Order,
                CreatedById = col.AuthorInfo.CreatedById.ToString(),
                UpdatedById = col.AuthorInfo.UpdatedById.ToString(),
                CreatedAt = col.Timestamps.CreatedAt,
                UpdatedAt = col.Timestamps.UpdatedAt
            })
        );

        return response;
    }
}

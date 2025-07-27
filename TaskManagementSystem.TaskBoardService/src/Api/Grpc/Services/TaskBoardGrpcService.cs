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
        var command = new RenameColumnCommand(
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

    public override async Task<TaskBoardListProtoResponse> GetByIds(TaskBoardListProtoRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Handling '{}' request with ids: {}", nameof(GetByIds), string.Join(", ", request.Ids));
        var query = new GetBoardsByIdsQuery(
            Ids: [..request.Ids.Select(Guid.Parse)]);

        var result = await _mediator.Send(query);

        if (result.IsFailure)
        {
            _logger.LogError("Request {} failed.", nameof(GetByIds));
            throw result.CreateExceptionFrom();
        }

        var response = new TaskBoardListProtoResponse();

        response.TaskBoards.AddRange(
            [
                ..result.Value.Boards.Select(b => new TaskBoardProto {
                    Id = b.Id.ToString(),
                    Name = b.Name,
                    Description = b.Description,
                    CreatedById = b.AuthorInfo.CreatedById.ToString(),
                    UpdatedById = b.AuthorInfo.UpdatedById.ToString(),
                    CreatedAt = b.Timestamps.CreatedAt,
                    UpdatedAt = b.Timestamps.UpdatedAt,
                })
            ]
        );

        return response;
    }

    public override async Task<TaskBoardDetailedProto> GetById(TaskBoardGetProtoRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Handling 'Get board by id'. Board id: {}", request.Id);
        var query = new GetBoardByIdQuery(Id: Guid.Parse(request.Id));

        var result = await _mediator.Send(query);

        if (result.IsFailure)
        {
            _logger.LogError("Failed to complete the query. Details in json format: {ErrorDetails}.", result.ErrorDetailsToJson());
            throw result.CreateExceptionFrom();
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

    public override async Task<TaskBoardGetAllColumnsResponse> GetAllColumns(TaskBoardGetAllColumnsRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Handling {} request with board id: {}", nameof(GetAllColumns), request.BoardId);

        var query = new GetAllColumnsQuery(
            BoardId: Guid.Parse(request.BoardId)
        );

        var result = await _mediator.Send(query);

        if (result.IsFailure)
        {
            _logger.LogError("Failed to complete the query. Details in json format: {}", result.ErrorDetailsToJson());
            throw result.CreateExceptionFrom();
        }

        var response = new TaskBoardGetAllColumnsResponse();

        response.Columns.AddRange(
            [
                ..result.Value.Columns.Select(c => new TaskBoardColumnProto {
                    Id = c.Id.ToString(),
                    Name = c.Name,
                    Order = c.Order,
                    CreatedById = c.AuthorInfo.CreatedById.ToString(),
                    UpdatedById = c.AuthorInfo.UpdatedById.ToString(),
                    CreatedAt = c.Timestamps.CreatedAt,
                })
            ]
        );

        return response;
    }

    public override async Task<Empty> Update(TaskBoardUpdateProtoRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Updating task board with ID: {TaskBoardId}. Provided data: {}", request.Id, request.ToString());

        var command = new UpdateBoardCommand(
            Id: Guid.Parse(request.Id),
            Name: request.Name,
            Description: request.Description
        );

        var result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            _logger.LogError("Failed to update task board. Details in json format: {ErrorDetails}", result.ErrorDetailsToJson());
            throw result.CreateExceptionFrom();
        }

        _logger.LogInformation("Updated task board with ID: {TaskBoardId}", request.Id);

        return new Empty();
    }

    public override async Task<Empty> Delete(TaskBoardDeleteProtoRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Deleting task board with ID: {}", request.Id);

        var command = new DeleteBoardCommand(
            Id: Guid.Parse(request.Id)
        );

        await _mediator.Send(command);

        return new Empty();
    }

    public override async Task<Empty> DeleteColumn(TaskBoardColumnDeleteProtoRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Deleting task board column with ID: {}", request.Id);

        var command = new DeleteColumnCommand(
            ColumnId: Guid.Parse(request.Id)
        );

        await _mediator.Send(command);

        _logger.LogInformation("Deleted task board column with ID: {ColumnId}", request.Id);

        return new Empty();
    }
}

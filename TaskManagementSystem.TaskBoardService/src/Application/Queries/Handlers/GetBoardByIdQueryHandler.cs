using MediatR;
using TaskManagementSystem.SharedLib.Enums.Exceptions;
using TaskManagementSystem.SharedLib.Exceptions;
using TaskManagementSystem.SharedLib.Handlers;
using TaskManagementSystem.TaskBoardService.Application.DTO;
using TaskManagementSystem.TaskBoardService.Application.Queries.Results;
using TaskManagementSystem.TaskBoardService.Core.Aggregates;
using TaskManagementSystem.TaskBoardService.Core.Interfaces.Repository;

namespace TaskManagementSystem.TaskBoardService.Application.Queries.Handlers;


public class GetBoardByIdQueryHandler : IRequestHandler<GetBoardByIdQuery, Result<GetBoardByIdQueryResult>>
{
    private readonly ITaskBoardRepository _boardRepository;

    public GetBoardByIdQueryHandler(ITaskBoardRepository boardRepository)
    {
        _boardRepository = boardRepository;
    }

    public async Task<Result<GetBoardByIdQueryResult>> Handle(GetBoardByIdQuery request, CancellationToken cancellationToken)
    {
        var board = await _boardRepository.GetByIdAsync(request.Id, cancellationToken);

        if (board == null)
        {
            throw new AppException(
                statusCode: AppExceptionStatusCode.NotFound,
                message: AppExceptionErrorMessages.NotFound
            );
        }

        var result = MapToResult(board);

        return Result<GetBoardByIdQueryResult>.Success(result);
    }

    private GetBoardByIdQueryResult MapToResult(TaskBoardAggregate board)
    {
        return new GetBoardByIdQueryResult (
            Id: board.Id,
            Name: board.Name,
            Description: board.Description,
            Timestamps: board.Timestamps,
            AuthorInfo: board.AuthorInfo,
            Columns: [
                ..board.Columns.Select(col => ColumnListDto.FromModel(col)
                )
            ]
        );
    }
}

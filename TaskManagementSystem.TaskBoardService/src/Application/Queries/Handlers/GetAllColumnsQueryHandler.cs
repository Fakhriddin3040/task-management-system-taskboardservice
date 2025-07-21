using MediatR;
using TaskManagementSystem.SharedLib.Handlers;
using TaskManagementSystem.TaskBoardService.Application.Queries.Results;
using TaskManagementSystem.TaskBoardService.Core.Interfaces.Repository;

namespace TaskManagementSystem.TaskBoardService.Application.Queries.Handlers;


public class GetAllColumnsQueryHandler : IRequestHandler<GetAllColumnsQuery, Result<GetAllColumnsQueryResult>>
{
    private ITaskBoardRepository _boardRepository;

    public GetAllColumnsQueryHandler(ITaskBoardRepository boardRepository)
    {
        _boardRepository = boardRepository;
    }

    public async Task<Result<GetAllColumnsQueryResult>> Handle(GetAllColumnsQuery request, CancellationToken cancellationToken)
    {
        var columns = await _boardRepository.GetColumnsAsync(
            taskBoardId: request.BoardId, cancellationToken: cancellationToken
        );

        return Result<GetAllColumnsQueryResult>.Success(
            GetAllColumnsQueryResult.FromModels(columns)
        );
    }
}

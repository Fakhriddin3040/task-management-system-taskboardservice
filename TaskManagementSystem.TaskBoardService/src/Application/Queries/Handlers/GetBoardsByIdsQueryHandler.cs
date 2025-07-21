using MediatR;
using TaskManagementSystem.SharedLib.Handlers;
using TaskManagementSystem.TaskBoardService.Application.Queries.Results;
using TaskManagementSystem.TaskBoardService.Core.Interfaces.Repository;

namespace TaskManagementSystem.TaskBoardService.Application.Queries.Handlers;


public sealed class GetBoardsByIdsQueryHandler : IRequestHandler<GetBoardsByIdsQuery, Result<GetBoardsByIdsQueryResult>>
{
    private readonly ITaskBoardRepository _repository;

    public GetBoardsByIdsQueryHandler(ITaskBoardRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<GetBoardsByIdsQueryResult>> Handle(GetBoardsByIdsQuery request, CancellationToken cancellationToken)
    {
        var boards = await _repository.GetByIdsAsync(
            ids: request.Ids,
            detailed: false,
            cancellationToken: cancellationToken);

        return Result<GetBoardsByIdsQueryResult>
            .Success(GetBoardsByIdsQueryResult.FromModels(boards));
    }
}

using MediatR;
using TaskManagementSystem.SharedLib.Handlers;
using TaskManagementSystem.TaskBoardService.Application.Queries.Results;

namespace TaskManagementSystem.TaskBoardService.Application.Queries;



public sealed record GetBoardsByIdsQuery(IEnumerable<Guid> Ids) : IRequest<Result<GetBoardsByIdsQueryResult>>;

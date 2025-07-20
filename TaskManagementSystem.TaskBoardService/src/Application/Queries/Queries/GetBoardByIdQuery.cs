using MediatR;
using TaskManagementSystem.SharedLib.Handlers;
using TaskManagementSystem.TaskBoardService.Application.Queries.Results;

namespace TaskManagementSystem.TaskBoardService.Application.Queries;


public sealed record GetBoardByIdQuery(Guid Id) : IRequest<Result<GetBoardByIdQueryResult>>;

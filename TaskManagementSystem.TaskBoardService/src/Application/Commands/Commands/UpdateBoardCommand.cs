using MediatR;
using TaskManagementSystem.SharedLib.Handlers;
using TaskManagementSystem.TaskBoardService.Application.Commands.Results;
using TaskManagementSystem.TaskBoardService.Core.Models;

namespace TaskManagementSystem.TaskBoardService.Application.Commands;


public sealed record UpdateBoardCommand(Guid Id, string Name, string Description) : IRequest<Result<UpdateBoardCommandResult>>;

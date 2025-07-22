using MediatR;

namespace TaskManagementSystem.TaskBoardService.Application.Commands;


public sealed record DeleteBoardCommand(Guid Id) : IRequest;

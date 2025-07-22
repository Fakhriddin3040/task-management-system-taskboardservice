using MediatR;

namespace TaskManagementSystem.TaskBoardService.Application.Commands;


public sealed record DeleteColumnCommand(Guid ColumnId) : IRequest;

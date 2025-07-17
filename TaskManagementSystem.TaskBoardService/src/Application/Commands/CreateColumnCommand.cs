using MediatR;
using TaskManagementSystem.SharedLib.Handlers;
using TaskManagementSystem.TaskBoardService.Application.Commands.Results;

namespace TaskManagementSystem.TaskBoardService.Application.Commands;


public class CreateColumnCommand : IRequest<Result<CreateColumnCommandResult>>
{
    public Guid BoardId { get; set; }
    public string Name { get; set; }

    public CreateColumnCommand(Guid boardId, string name)
    {
        BoardId = boardId;
        Name = name;
    }
}

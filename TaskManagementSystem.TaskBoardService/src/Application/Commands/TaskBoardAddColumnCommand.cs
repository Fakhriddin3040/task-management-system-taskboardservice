using MediatR;
using TaskManagementSystem.SharedLib.Handlers;
using TaskManagementSystem.TaskBoardService.Application.Results;

namespace TaskManagementSystem.TaskBoardService.Application.Commands;


public class TaskBoardAddColumnCommand : IRequest<Result<TaskBoardAddColumnResult>>
{
    public Guid BoardId { get; set; }
    public string Name { get; set; }

    public TaskBoardAddColumnCommand(Guid boardId, string name)
    {
        BoardId = boardId;
        Name = name;
    }
}

using MediatR;
using TaskManagementSystem.SharedLib.Handlers;
using TaskManagementSystem.TaskBoardService.Application.Results;

namespace TaskManagementSystem.TaskBoardService.Application.Commands;


public class TaskBoardCreateCommand : IRequest<Result<TaskBoardCreateResult>>
{
    public string Name { get; set; }
    public string? Description { get; set; }

    public TaskBoardCreateCommand(string name, string? description)
    {
        Name = name;
        Description = description;
    }
}

using MediatR;
using TaskManagementSystem.SharedLib.Handlers;
using TaskManagementSystem.TaskBoardService.Application.Commands.Results;

namespace TaskManagementSystem.TaskBoardService.Application.Commands;


public class CreateBoardCommand : IRequest<Result<CreateBoardCommandResult>>
{
    public string Name { get; set; }
    public string? Description { get; set; }

    public CreateBoardCommand(string name, string? description)
    {
        Name = name;
        Description = description;
    }
}

using MediatR;
using TaskManagementSystem.SharedLib.Handlers;
using Unit = TaskManagementSystem.SharedLib.Structs.Unit;

namespace TaskManagementSystem.TaskBoardService.Application.Commands;


public sealed record UpdateColumnCommand : IRequest<Result<Unit>>
{
    public Guid ColumnId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Order { get; set; }

    public UpdateColumnCommand(Guid columnId, string name, int order)
    {
        ColumnId = columnId;
        Name = name;
        Order = order;
    }
}

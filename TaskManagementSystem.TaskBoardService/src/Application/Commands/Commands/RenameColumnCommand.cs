using MediatR;
using TaskManagementSystem.SharedLib.Handlers;
using Unit = TaskManagementSystem.SharedLib.Structs.Unit;

namespace TaskManagementSystem.TaskBoardService.Application.Commands;


public sealed record RenameColumnCommand : IRequest<Result<Unit>>
{
    public Guid ColumnId { get; set; }
    public string Name { get; set; } = string.Empty;
    public long Order { get; set; }

    public RenameColumnCommand(Guid columnId, string name, long order)
    {
        ColumnId = columnId;
        Name = name;
        Order = order;
    }
}

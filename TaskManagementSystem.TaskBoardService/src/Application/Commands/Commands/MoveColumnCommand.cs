using MediatR;
using TaskManagementSystem.SharedLib.Handlers;
using Unit = TaskManagementSystem.SharedLib.Structs.Unit;

namespace TaskManagementSystem.TaskBoardService.Application.Commands;


public readonly struct MoveColumnCommand(Guid columnId, Guid previousId, Guid nextId) : IRequest
{
    public Guid ColumnId { get; } = columnId;
    public Guid PreviousId { get; } = previousId;
    public Guid NextId { get; } = nextId;
}

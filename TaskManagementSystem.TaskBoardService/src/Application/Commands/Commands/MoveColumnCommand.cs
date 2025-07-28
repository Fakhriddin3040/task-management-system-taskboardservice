using MediatR;
using TaskManagementSystem.SharedLib.Handlers;
using Unit = TaskManagementSystem.SharedLib.Structs.Unit;

namespace TaskManagementSystem.TaskBoardService.Application.Commands;


public readonly struct MoveColumnCommand(Guid ColumnId, Guid PreviousId, Guid nextId) : IRequest<Result<Unit>>
{
    public Guid ColumnId { get; } = ColumnId;
    public Guid PreviousId { get; } = PreviousId;
    public Guid NextId { get; } = nextId;
}

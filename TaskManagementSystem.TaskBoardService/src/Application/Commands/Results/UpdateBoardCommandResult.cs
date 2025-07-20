namespace TaskManagementSystem.TaskBoardService.Application.Commands.Results;


public sealed record UpdateBoardCommandResult(Guid BoardId, string Name, string Description)
{
    public required Guid BoardId { get; init; } = BoardId;
    public required string Name { get; init; } = Name;
    public required string Description { get; init; } = Description;

}

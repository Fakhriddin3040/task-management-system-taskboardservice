namespace TaskManagementSystem.TaskBoardService.Application.Commands.Results;


public sealed record UpdateBoardCommandResult
{
    public required Guid BoardId { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required DateTimeOffset UpdatedAt { get; init; }

    public UpdateBoardCommandResult(Guid boardId, string name, string description, DateTimeOffset updatedAt)
    {
        BoardId = boardId;
        Name = name;
        Description = description;
        UpdatedAt = updatedAt;
    }
}

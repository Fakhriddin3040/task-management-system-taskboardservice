namespace TaskManagementSystem.TaskBoardService.Application.Commands.Results;


public class CreateColumnCommandResult(Guid id, int order)
{
    public Guid Id { get; set; } = id;
    public int Order { get; set; } = order;
}

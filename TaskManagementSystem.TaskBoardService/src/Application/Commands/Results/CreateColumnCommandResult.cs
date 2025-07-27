namespace TaskManagementSystem.TaskBoardService.Application.Commands.Results;


public class CreateColumnCommandResult(Guid id, long order)
{
    public Guid Id { get; set; } = id;
    public long Order { get; set; } = order;
}

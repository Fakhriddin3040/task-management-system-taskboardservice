namespace TaskManagementSystem.TaskBoardService.Application.Results;


public class TaskBoardAddColumnResult(Guid id, int order)
{
    public Guid Id { get; set; } = id;
    public int Order { get; set; } = order;
}

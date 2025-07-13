namespace TaskManagementSystem.TaskBoardService.Application.Results;


public class TaskBoardCreateResult
{
    public Guid TaskBoardId { get; set; }

    public TaskBoardCreateResult(Guid taskBoardId)
    {
        TaskBoardId = taskBoardId;
    }
}

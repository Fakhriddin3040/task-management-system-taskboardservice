namespace TaskManagementSystem.TaskBoardService.Application.Commands.Results;


public class CreateBoardCommandResult
{
    public Guid TaskBoardId { get; set; }

    public CreateBoardCommandResult(Guid taskBoardId)
    {
        TaskBoardId = taskBoardId;
    }
}

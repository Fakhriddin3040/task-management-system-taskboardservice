using TaskManagementSystem.AuthService.Core.ValueObjects;
using TaskManagementSystem.SharedLib.ValueObjects;

namespace TaskManagementSystem.TaskBoardService.Core.Models;


public class TaskBoardModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid OrganizationId { get; set; }
    public AuthorInfo AuthorInfo { get; set; }
    public Timestamps Timestamps { get; set; }
    public List<TaskColumnModel> TaskColumns { get; set; }
}

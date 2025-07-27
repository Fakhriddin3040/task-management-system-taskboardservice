using TaskManagementSystem.AuthService.Core.ValueObjects;
using TaskManagementSystem.SharedLib.ValueObjects;

namespace TaskManagementSystem.TaskBoardService.Core.Models;


public class TaskBoardColumnModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public long Order { get; set; }
    public Guid BoardId { get; set; }
    public AuthorInfo AuthorInfo { get; set; }
    public Timestamps Timestamps { get; set; }
}

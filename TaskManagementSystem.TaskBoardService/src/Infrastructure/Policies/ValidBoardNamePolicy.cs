using TaskManagementSystem.TaskBoardService.Core.Interfaces.Policies;

namespace TaskManagementSystem.TaskBoardService.Infrastructure.Policies;


public class ValidBoardNamePolicy : IValidBoardNamePolicy
{
    public bool IsValid(string boardName)
    {
        return !string.IsNullOrWhiteSpace(boardName) && boardName.Length <= 30 && !boardName.Any(char.IsControl) &&
               !boardName.Any(c => char.IsWhiteSpace(c) && c != ' ');
    }
}

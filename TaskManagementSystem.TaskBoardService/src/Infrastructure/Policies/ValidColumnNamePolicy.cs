using TaskManagementSystem.TaskBoardService.Core.Interfaces.Policies;

namespace TaskManagementSystem.TaskBoardService.Infrastructure.Policies;


public class ValidColumnNamePolicy : IValidColumnNamePolicy
{
    public bool IsValid(string name)
    {
        return ! string.IsNullOrEmpty(name) && name.Length is > 5 and <= 50 && !name.Any(char.IsControl) &&
               !name.Any(c => char.IsWhiteSpace(c) && c != ' ' && c != '\t') &&
               !name.Any(c => char.IsPunctuation(c) && c != '-' && c != '_');
    }
}

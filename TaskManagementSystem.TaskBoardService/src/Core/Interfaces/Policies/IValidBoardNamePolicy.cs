namespace TaskManagementSystem.TaskBoardService.Core.Interfaces.Policies;


public interface IValidBoardNamePolicy
{
    /// <summary>
    /// Validates the board name.
    /// </summary>
    /// <param name="boardName">The board name to validate.</param>
    /// <returns>True if the board name is valid, otherwise false.</returns>
    bool IsValid(string boardName);
}

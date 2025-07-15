using TaskManagementSystem.AuthService.Core.ValueObjects;
using TaskManagementSystem.SharedLib.Abstractions.Interfaces;
using TaskManagementSystem.SharedLib.Abstractions.Interfaces.Policies;
using TaskManagementSystem.SharedLib.Enums.Exceptions;
using TaskManagementSystem.SharedLib.Exceptions;
using TaskManagementSystem.SharedLib.Handlers;
using TaskManagementSystem.SharedLib.Models.Fields;
using TaskManagementSystem.SharedLib.ValueObjects;
using TaskManagementSystem.TaskBoardService.Core.Models;

namespace TaskManagementSystem.TaskBoardService.Aggregates;


public sealed class TaskBoardColumnAggregate : TaskBoardColumnModel
{
    private TaskBoardColumnAggregate() {}

    public static async Task<Result<TaskBoardColumnAggregate>> CreateAsync(
        string name,
        int order,
        Guid boardId,
        Guid createdById,
        CancellationToken cancellationToken,
        IDateTimeService dateTimeService,
        IValidNamePolicy namePolicy,
        ITaskBoardExistsByIdPolicy taskBoardExistsByIdPolicy
        )
    {
        var errors = new List<AppExceptionDetail>();

        if (!namePolicy.IsValid(name))
        {
            errors.Add(
                new(
                    AppExceptionStatusCode.InvalidFormat,
                    TaskColumnField.Name
                )
            );
        }

        if (!await taskBoardExistsByIdPolicy.ExistsAsync(boardId, cancellationToken))
        {
            errors.Add(
                new(
                    AppExceptionStatusCode.NotFound,
                    TaskColumnField.Board.AsForeignKey()
                )
            );
        }

        if (errors.Any())
        {
            return Result<TaskBoardColumnAggregate>.Failure(errors);
        }

        var timestamp = new Timestamps(dateTimeService);
        var authorInfo = new AuthorInfo(createdById, createdById);

        var taskColumn = new TaskBoardColumnAggregate
        {
            Id = Guid.NewGuid(),
            Name = name,
            Order = order,
            BoardId = boardId,
            Timestamps = timestamp,
            AuthorInfo = authorInfo
        };

        return Result<TaskBoardColumnAggregate>.Success(taskColumn);
    }
}

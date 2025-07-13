using TaskManagementSystem.AuthService.Core.ValueObjects;
using TaskManagementSystem.SharedLib.Abstractions.Interfaces;
using TaskManagementSystem.SharedLib.Enums.Exceptions;
using TaskManagementSystem.SharedLib.Exceptions;
using TaskManagementSystem.SharedLib.Handlers;
using TaskManagementSystem.SharedLib.Models.Fields;
using TaskManagementSystem.SharedLib.ValueObjects;
using TaskManagementSystem.TaskBoardService.Core.Interfaces.Policies;
using TaskManagementSystem.TaskBoardService.Core.Models;

namespace TaskManagementSystem.TaskBoardService.Core.Aggregates;

public sealed class TaskBoardAggregate : TaskBoardModel
{
    private TaskBoardAggregate()
    {
        // Private constructor to prevent instantiation without using the factory method
    }

    public static async Task<Result<TaskBoardAggregate>> Create(
        string name,
        string description,
        Guid organizationId,
        Guid createdById,
        CancellationToken cancellationToken,
        IDateTimeService dateTimeService,
        IValidNamePolicy namePolicy,
        IUniqueTaskBoardNamePolicy uniqueNamePolicy,
        IOrganizationExistsPolicy organizationExistsPolicy
        )
    {
        var errors = new List<AppExceptionDetail>();

        if (!namePolicy.IsValid(name))
        {
            errors.Add(
                new(
                    StatusCode: AppExceptionStatusCode.InvalidFormat,
                    Field: TaskBoardField.Name
                )
            );
        }

        if (! await uniqueNamePolicy.IsUnique(name))
        {
            errors.Add(
                new(
                    StatusCode: AppExceptionStatusCode.UniqueConstraints,
                    Field: TaskBoardField.Name
                )
            );
        }

        if (! await organizationExistsPolicy.ExistsAsync(organizationId, cancellationToken))
        {
            errors.Add(
                new(
                    StatusCode: AppExceptionStatusCode.NotFound,
                    Field: TaskBoardField.Organization.AsForeignKey()
                )
            );
        }

        if (errors.Any())
        {
            return Result<TaskBoardAggregate>.Failure(errors);
        }

        var timestamps = new Timestamps(dateTimeService);
        var authorInfo = new AuthorInfo(createdById, createdById);

        var taskBoard = new TaskBoardAggregate {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            OrganizationId = organizationId,
            AuthorInfo = authorInfo,
            Timestamps = timestamps,
        };

        return Result<TaskBoardAggregate>.Success(taskBoard);
    }
}

using TaskManagementSystem.SharedLib.Abstractions.Interfaces;
using TaskManagementSystem.AuthService.Core.ValueObjects;
using TaskManagementSystem.SharedLib.Algorithms.NumeralRank;
using TaskManagementSystem.SharedLib.Algorithms.NumeralRank.Interfaces;
using TaskManagementSystem.SharedLib.Enums.Exceptions;
using TaskManagementSystem.SharedLib.Exceptions;
using TaskManagementSystem.SharedLib.Handlers;
using TaskManagementSystem.SharedLib.Models.Fields;
using TaskManagementSystem.SharedLib.Structs;
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

    public static async Task<Result<TaskBoardAggregate>> CreateAsync(
        string name,
        string description,
        Guid organizationId,
        Guid createdById,
        CancellationToken cancellationToken,
        IDateTimeService dateTimeService,
        IValidBoardNamePolicy namePolicy,
        IUniqueBoardNamePolicy uniqueNamePolicy
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

        if (! await uniqueNamePolicy.IsUnique(name, cancellationToken))
        {
            errors.Add(
                new(
                    StatusCode: AppExceptionStatusCode.UniqueConstraints,
                    Field: TaskBoardField.Name
                )
            );
        }

        if (errors.Any())
        {
            return Result<TaskBoardAggregate>.Failure(errors);
        }

        var timestamps = Timestamps.FromDateTimeService(dateTimeService);
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

    public async Task<Result<Unit>> UpdateAsync(
        string name,
        string description,
        Guid updateById,
        IDateTimeService dateTimeService,
        IValidBoardNamePolicy namePolicy,
        IUniqueBoardNamePolicy uniqueNamePolicy,
        CancellationToken cancellationToken)
    {
        var errors = new List<AppExceptionDetail>();

        if (!string.IsNullOrEmpty(name) && Name != name)
        {
            var isNameValid = namePolicy.IsValid(name);

            if (!isNameValid)
            {
                errors.Add(
                    new(
                        StatusCode: AppExceptionStatusCode.InvalidFormat,
                        Field: TaskBoardField.Name
                    )
                );
            }
            if (isNameValid && !await uniqueNamePolicy.IsUnique(name, cancellationToken))
            {
                isNameValid = false;
                errors.Add(
                    new(
                        StatusCode: AppExceptionStatusCode.UniqueConstraints,
                        Field: TaskBoardField.Name
                    )
                );
            }
            if (isNameValid)
            {
                Name = name;
            }
        }

        if (!string.IsNullOrEmpty(description))
        {
            Description = description;
        }

        if (errors.Any())
        {
            return Result<Unit>.Failure(errors);
        }

        Timestamps.Touch(dateTimeService);
        AuthorInfo.Update(updateById);

        return Result<Unit>.Success(Unit.Value);
    }

    public async Task<Result<TaskBoardColumnModel>> AddColumnAsync(
        string name,
        Guid createdById,
        CancellationToken cancellationToken,
        IDateTimeService dateTimeService,
        IValidColumnNamePolicy namePolicy,
        IUniqueColumnNamePolicy uniqueNamePolicy
    )
    {
        var errors = new List<AppExceptionDetail>();

        if (!namePolicy.IsValid(name))
        {
            errors.Add(
                new(
                    StatusCode: AppExceptionStatusCode.InvalidFormat,
                    Field: TaskColumnField.Name
                )
            );
        }

        if (
            !await uniqueNamePolicy.IsUniqueAsync(
                taskBoardId: Id,
                columnName: name,
                cancellationToken: cancellationToken,
                columnId: null)
            )
        {
            errors.Add(
                new(
                    StatusCode: AppExceptionStatusCode.UniqueConstraints,
                    Field: TaskColumnField.Name
                )
            );
        }

        if (errors.Any())
        {
            return Result<TaskBoardColumnModel>.Failure(errors);
        }

        var timestamps = Timestamps.FromDateTimeService(dateTimeService);
        var authorInfo = new AuthorInfo(createdById, createdById);

        var latestColumnsOrder = 100;

        var column = new TaskBoardColumnModel
        {
            BoardId = Id,
            Id = Guid.Empty,
            Name = name,
            AuthorInfo = authorInfo,
            Timestamps = timestamps,
            Order = latestColumnsOrder
        };

        Columns.Add(column);

        return Result<TaskBoardColumnModel>.Success(column);
    }

    public async Task<Result<TaskBoardColumnModel>> RenameColumnAsync(
        Guid columnId,
        string newName,
        Guid updatedById,
        CancellationToken cancellationToken,
        IValidColumnNamePolicy namePolicy,
        IUniqueColumnNamePolicy uniqueNamePolicy,
        IDateTimeService dateTimeService
        )
    {
        var column = GetColumnById(columnId);

        var errors = new List<AppExceptionDetail>();

        if (newName == column.Name)
        {
            return Result<TaskBoardColumnModel>.Success(column);
        }

        if (!namePolicy.IsValid(newName))
        {
            errors.Add(
                new(
                    AppExceptionStatusCode.InvalidFormat,
                    TaskColumnField.Name
                )
            );
        }

        if (
            !await uniqueNamePolicy.IsUniqueAsync(
                taskBoardId: Id,
                columnName: newName,
                cancellationToken: cancellationToken,
                columnId: column.Id)
            )
        {
            errors.Add(
                new(
                    AppExceptionStatusCode.UniqueConstraints,
                    TaskColumnField.Name
                )
            );
        }

        if (errors.Any())
        {
            return Result<TaskBoardColumnModel>.Failure(errors);
        }

        column.Name = newName;
        column.Timestamps.Touch(dateTimeService);
        column.AuthorInfo.Update(updatedById);

        return Result<TaskBoardColumnModel>.Success(column);
    }

    private TaskBoardColumnModel GetColumnById(Guid columnId)
    {
        return Columns.FirstOrDefault(c => c.Id == columnId)
            ?? throw new AppException(
                statusCode: AppExceptionStatusCode.NotFound,
                message: AppExceptionErrorMessages.NotFound
            );
    }

    public NumeralRankResult MoveColumn(
        Guid columnId,
        Guid updatedById,
        NumeralRankContext rankContext,
        IDateTimeService dateTimeService,
        INumeralRankStrategySelector rankStrategySelector
        )
    {
        var strategy = rankStrategySelector.GetStrategy(rankContext);
        var numeralRankResult = strategy.GenerateRank(rankContext);

        if (!numeralRankResult.IsValid)
        {
            return numeralRankResult;
        }

        var column = GetColumnById(columnId);

        column.Order = numeralRankResult.Rank;
        column.Timestamps.Touch(dateTimeService);
        column.AuthorInfo.Update(updatedById);

        Timestamps.Touch(dateTimeService);
        AuthorInfo.Update(updatedById);

        return numeralRankResult;
    }

    public void RemoveColumn(Guid columnId, Guid updatedById, IDateTimeService dateTimeService)
    {
        var column = GetColumnById(columnId);

        Columns.Remove(column);
        Timestamps.Touch(dateTimeService);
        AuthorInfo.Update(updatedById);
    }

    public void Delete() {}
}

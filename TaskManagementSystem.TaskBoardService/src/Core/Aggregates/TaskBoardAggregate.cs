using TaskManagementSystem.SharedLib.Abstractions.Interfaces;
using TaskManagementSystem.AuthService.Core.ValueObjects;
using TaskManagementSystem.SharedLib.Enums.Exceptions;
using TaskManagementSystem.SharedLib.Exceptions;
using TaskManagementSystem.SharedLib.Extensions;
using TaskManagementSystem.SharedLib.Handlers;
using TaskManagementSystem.SharedLib.Models.Fields;
using TaskManagementSystem.SharedLib.Structs;
using TaskManagementSystem.SharedLib.ValueObjects;
using TaskManagementSystem.TaskBoardService.Core.Extensions;
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

        var latestColumnsOrder = this.GetNextColumnOrder();

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

    public async Task<Result<TaskBoardColumnModel>> UpdateColumnAsync(
        Guid columnId,
        Guid updatedById,
        string name,
        CancellationToken cancellationToken,
        IValidColumnNamePolicy namePolicy,
        IUniqueColumnNamePolicy uniqueNamePolicy,
        IDateTimeService dateTimeService,
        int order = 0
        )
    {
        var column = Columns.FirstOrDefault(c => c.Id == columnId);

        if (column == null)
        {
            throw new AppException(
                statusCode: AppExceptionStatusCode.NotFound,
                message: AppExceptionErrorMessages.NotFound
            );
        }
        var result = Result<TaskBoardColumnModel>.Empty();

        if (!string.IsNullOrEmpty(name))
        {
            var localResult = await RenameColumnAsync(
                column: column,
                newName: name,
                updatedById: updatedById,
                cancellationToken: cancellationToken,
                namePolicy: namePolicy,
                uniqueNamePolicy: uniqueNamePolicy,
                dateTimeService: dateTimeService
            );

            if (localResult.IsFailure && localResult.ErrorDetails.Any())
            {
                result = localResult;
            }
        }

        if (order != 0)
        {
            var localResult = ReorderColumn(
                column: column,
                newOrder: order,
                updatedById: updatedById,
                dateTimeService: dateTimeService
            );
            if (localResult.IsFailure && localResult.ErrorDetails.Any())
            {
                result = result.Merge(localResult);
            }
        }

        if (result.ErrorDetails.Any())
        {
            return result;
        }

        AuthorInfo.UpdatedById = updatedById;
        Timestamps.Touch(dateTimeService);

        return Result<TaskBoardColumnModel>.Success(column);
    }

    private async Task<Result<TaskBoardColumnModel>> RenameColumnAsync(
        TaskBoardColumnModel column,
        string newName,
        Guid updatedById,
        CancellationToken cancellationToken,
        IValidColumnNamePolicy namePolicy,
        IUniqueColumnNamePolicy uniqueNamePolicy,
        IDateTimeService dateTimeService
        )
    {
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

    private Result<TaskBoardColumnModel> ReorderColumn(
        TaskBoardColumnModel column,
        int newOrder,
        Guid updatedById,
        IDateTimeService dateTimeService
        )
    {
        if (newOrder == column.Order)
        {
            return Result<TaskBoardColumnModel>.Success(column);
        }

        var errors = new List<AppExceptionDetail>();

        if (newOrder < 0)
        {
            return Result<TaskBoardColumnModel>.Failure(errors);
        }

        if (newOrder > Columns.Count)
        {
            errors.Add(
                new(
                    AppExceptionStatusCode.InvalidValue,
                    TaskColumnField.Order
                )
            );
        }

        if (errors.Any())
        {
            return Result<TaskBoardColumnModel>.Failure(errors);
        }

        if (newOrder > column.Order)
        {
            ModifyColumns(
                Columns.Where(c => c.Order > column.Order && c.Order <= newOrder)
                    .ToList(), dateTimeService: dateTimeService, updatedById: updatedById, col => col.Order++
            );
        }
        else
        {
            ModifyColumns(
                columns: Columns.Where(c => c.Order < column.Order && c.Order >= newOrder)
                    .ToList(),
                dateTimeService: dateTimeService,
                updatedById: updatedById,
                col => col.Order--
            );
        }

        column.Order = newOrder;
        column.Timestamps.Touch(dateTimeService);
        column.AuthorInfo.Update(updatedById);

        return Result<TaskBoardColumnModel>.Success(column);
    }

    private void ModifyColumns(
        List<TaskBoardColumnModel> columns,
        IDateTimeService dateTimeService,
        Guid updatedById,
        Action<TaskBoardColumnModel> modifyAction
    )
    {
        columns.ForEach(c =>
        {
            modifyAction(c);
            c.Timestamps.Touch(dateTimeService);
            c.AuthorInfo.Update(updatedById);
        });
    }

    public void RemoveColumn(Guid columnId, Guid updatedById, IDateTimeService dateTimeService)
    {
        var column = Columns.FirstOrDefault(c => c.Id == columnId);

        if (column == null)
        {
            throw new AppException(
                statusCode: AppExceptionStatusCode.NotFound,
                message: AppExceptionErrorMessages.NotFound
            );
        }

        var columnsToMove = Columns.Where(c => c.Order > column.Order).ToList();

        if (columnsToMove.Any())
        {
            ModifyColumns(
                columns: columnsToMove,
                dateTimeService: dateTimeService,
                updatedById: updatedById,
                col => col.Order-- // Decrease order of columns after the removed one
            );
        }

        Columns.Remove(column);
        Timestamps.Touch(dateTimeService);
        AuthorInfo.Update(updatedById);
    }

    public void Delete() {}
}

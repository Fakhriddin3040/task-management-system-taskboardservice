using TaskManagementSystem.AuthService.Core.ValueObjects;
using TaskManagementSystem.SharedLib.ValueObjects;
using TaskManagementSystem.TaskBoardService.Core.Models;

namespace TaskManagementSystem.TaskBoardService.Application.DTO;


public sealed record ColumnListDto(
    Guid Id,
    string Name,
    int Order,
    AuthorInfo AuthorInfo,
    Timestamps Timestamps)
{
    public static ColumnListDto FromModel(TaskBoardColumnModel model)
    {
        return new (
            Id: model.Id,
            Name: model.Name,
            Order: model.Order,
            Timestamps: model.Timestamps,
            AuthorInfo: model.AuthorInfo
        );
    }
}

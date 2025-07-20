using TaskManagementSystem.TaskBoardService.Core.Models;

namespace TaskManagementSystem.TaskBoardService.Application.DTO;


public sealed record ColumnListDto(
    Guid Id,
    string Name,
    int Order,
    Guid CreatedById,
    Guid UpdatedById,
    DateTime CreatedAt,
    DateTime UpdatedAt
)
{
    public static ColumnListDto FromModel(TaskBoardColumnModel model)
    {
        return new (
            Id: model.Id,
            Name: model.Name,
            Order: model.Order,
            CreatedById: model.AuthorInfo.CreatedById,
            UpdatedById: model.AuthorInfo.UpdatedById,
            CreatedAt: model.Timestamps.CreatedAt,
            UpdatedAt: model.Timestamps.UpdatedAt
        );
    }
}

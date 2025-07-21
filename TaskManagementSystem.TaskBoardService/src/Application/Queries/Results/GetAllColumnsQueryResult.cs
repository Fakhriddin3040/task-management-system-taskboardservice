using TaskManagementSystem.TaskBoardService.Application.DTO;
using TaskManagementSystem.TaskBoardService.Core.Models;

namespace TaskManagementSystem.TaskBoardService.Application.Queries.Results;


public sealed record GetAllColumnsQueryResult(IEnumerable<ColumnListDto> Columns)
{
    public static GetAllColumnsQueryResult FromModels(IEnumerable<TaskBoardColumnModel> columns)
    {
        return new GetAllColumnsQueryResult(
            Columns: [
                ..columns.Select(
                    m => new ColumnListDto(
                        Id: m.Id,
                        Name: m.Name,
                        Order: m.Order,
                        Timestamps: m.Timestamps,
                        AuthorInfo: m.AuthorInfo
                    )
                )
            ]
        );
    }
}

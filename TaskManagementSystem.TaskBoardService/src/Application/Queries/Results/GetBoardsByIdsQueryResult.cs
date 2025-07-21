using TaskManagementSystem.TaskBoardService.Application.DTO;
using TaskManagementSystem.TaskBoardService.Core.Models;

namespace TaskManagementSystem.TaskBoardService.Application.Queries.Results;


public record GetBoardsByIdsQueryResult(IEnumerable<BoardListDto> Boards)
{
    public static GetBoardsByIdsQueryResult FromModels(IEnumerable<TaskBoardModel> models)
    {
        return new(
            Boards: [
                ..models.Select(b => new BoardListDto(
                    Id: b.Id,
                    Name: b.Name,
                    Description: b.Description,
                    Timestamps: b.Timestamps,
                    AuthorInfo: b.AuthorInfo
                ))
            ]
        );
    }
}

using TaskManagementSystem.TaskBoardService.Application.DTO;

namespace TaskManagementSystem.TaskBoardService.Application.Queries.Results;


public sealed record GetBoardByIdQueryResult(
    Guid Id,
    string Name,
    string Description,
    Guid CreatedById,
    Guid UpdatedById,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    List<ColumnListDto> Columns) : BoardDetailedDto(Id, Name, Description, CreatedById, UpdatedById, CreatedAt, UpdatedAt, Columns);

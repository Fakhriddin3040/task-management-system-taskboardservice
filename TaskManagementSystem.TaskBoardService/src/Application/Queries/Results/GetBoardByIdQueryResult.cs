using TaskManagementSystem.AuthService.Core.ValueObjects;
using TaskManagementSystem.SharedLib.ValueObjects;
using TaskManagementSystem.TaskBoardService.Application.DTO;

namespace TaskManagementSystem.TaskBoardService.Application.Queries.Results;


public sealed record GetBoardByIdQueryResult(
    Guid Id,
    string Name,
    string Description,
    Timestamps Timestamps,
    AuthorInfo AuthorInfo,
    List<ColumnListDto> Columns) : BoardDetailedDto(Id, Name, Description, Timestamps, AuthorInfo, Columns);

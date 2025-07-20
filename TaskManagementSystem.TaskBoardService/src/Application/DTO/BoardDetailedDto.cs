using TaskManagementSystem.AuthService.Core.ValueObjects;
using TaskManagementSystem.SharedLib.ValueObjects;

namespace TaskManagementSystem.TaskBoardService.Application.DTO;


public abstract record BoardDetailedDto(
    Guid Id,
    string Name,
    string Description,
    Timestamps Timestamps,
    AuthorInfo AuthorInfo,
    List<ColumnListDto> Columns
);

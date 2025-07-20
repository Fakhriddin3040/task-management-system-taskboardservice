namespace TaskManagementSystem.TaskBoardService.Application.DTO;


public abstract record BoardDetailedDto(
    Guid Id,
    string Name,
    string Description,
    Guid CreatedById,
    Guid UpdatedById,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    List<ColumnListDto> Columns
);

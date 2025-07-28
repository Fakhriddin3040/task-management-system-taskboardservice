using MediatR;
using TaskManagementSystem.SharedLib.Algorithms.NumeralRank;
using TaskManagementSystem.SharedLib.Algorithms.NumeralRank.Interfaces;
using TaskManagementSystem.SharedLib.Enums.Exceptions;
using TaskManagementSystem.SharedLib.Exceptions;
using TaskManagementSystem.SharedLib.Handlers;
using TaskManagementSystem.TaskBoardService.Core.Interfaces.Repository;
using TaskManagementSystem.TaskBoardService.Core.Models;
using Unit = TaskManagementSystem.SharedLib.Structs.Unit;

namespace TaskManagementSystem.TaskBoardService.Application.Commands.Handlers;


public class MoveColumnCommandHandler : IRequestHandler<MoveColumnCommand, Result<Unit>>
{
    private readonly ITaskBoardRepository _boardRepository;
    private readonly INumeralRankValidationStrategySelector _rankValidationStrategySelector;

    public async Task<Result<Unit>> Handle(MoveColumnCommand request, CancellationToken cancellationToken)
    {
        var board = await _boardRepository.GetByColumnIdAsync(request.ColumnId, cancellationToken);

        if (board is null)
        {
            throw new AppException(
                statusCode: AppExceptionStatusCode.NotFound,
                message: AppExceptionErrorMessages.NotFound
            );
        }

        var
    }

    private async Result<Unit> ValidateColumnMove(
        IEnumerable<TaskBoardColumnModel> columns,
        Guid columnId,
        int newRank,
        CancellationToken cancellationToken)
    {
        var requiredColumns = columns.FirstOrDefault(c => c.Id == columnId);

        if (requiredColumns is null)
        {
            throw new AppException(
                statusCode: AppExceptionStatusCode.NotFound,
                message: AppExceptionErrorMessages.NotFound
            );
        }

        var validationStrategy = _rankValidationStrategySelector.GetValidationStrategy(columns);

        return await validationStrategy.ValidateAsync();
    }

    private NumeralRankContext GetRankingContext(
        IEnumerable<TaskBoardColumnModel> columns,
        MoveColumnCommand request
        )
    {
        long prevRank = NumeralRankOptions.Empty;
        long nextRank = NumeralRankOptions.Empty;

        var orderedColumns = columns.OrderBy(c => c.Order);

        var targetColIdx = orderedColumns.OrderBy(c => c.Order)
            .Select((item, idx) => new {
                item.Id,
                item.Order,
                idx
            })
            .FirstOrDefault(item => item.Id == request.ColumnId)?.idx ?? -1;

        if (targetColIdx == -1)
        {
            throw new InvalidProgramException(
                $"Column with ID {request.ColumnId} not found in the provided columns, but was checked in handler."
            );
        }

        var requiredColumns = orderedColumns.Skip(targetColIdx - 1).Take(targetColIdx + 1).ToList();

        return new NumeralRankContext(
            previousRank: requiredColumns[0]?.Order ?? NumeralRankOptions.Empty,
            nextRank: requiredColumns[2]?.Order ?? NumeralRankOptions.Empty
        );
    }
}

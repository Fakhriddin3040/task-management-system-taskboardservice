using MediatR;
using TaskManagementSystem.SharedLib.Abstractions.Interfaces;
using TaskManagementSystem.SharedLib.Exceptions;
using TaskManagementSystem.SharedLib.Providers.Interfaces;
using TaskManagementSystem.TaskBoardService.Core.Interfaces.Repository;
using ExecutionContext = TaskManagementSystem.SharedLib.DTO.ExecutionContext;

namespace TaskManagementSystem.TaskBoardService.Application.Commands.Handlers;


public sealed class DeleteColumnCommandHandler : IRequestHandler<DeleteColumnCommand>
{
    private readonly ITaskBoardRepository _boardRepository;
    private readonly IDateTimeService _dateTimeService;
    private readonly ExecutionContext _context;

    public DeleteColumnCommandHandler(ITaskBoardRepository taskBoardRepository, IDateTimeService dateTimeService, IExecutionContextProvider contextProvider)
    {
        _boardRepository = taskBoardRepository ?? throw new ArgumentNullException(nameof(taskBoardRepository));
        _dateTimeService = dateTimeService ?? throw new ArgumentNullException(nameof(dateTimeService));
        _context = contextProvider.GetContext();
    }

    public async Task Handle(DeleteColumnCommand request, CancellationToken cancellationToken)
    {
        var board = await _boardRepository.GetByColumnIdAsync(request.ColumnId, cancellationToken);

        if (board == null)
        {
            throw AppException.NotFound();
        }

        board.RemoveColumn(
            columnId: request.ColumnId,
            dateTimeService: _dateTimeService,
            updatedById: _context.User.Id
        );

        _boardRepository.Update(board);

        var result = await _boardRepository.SaveChangesAsync(cancellationToken);

        if (result == 0)
        {
            throw AppException.InternalServerError();
        }
    }
}

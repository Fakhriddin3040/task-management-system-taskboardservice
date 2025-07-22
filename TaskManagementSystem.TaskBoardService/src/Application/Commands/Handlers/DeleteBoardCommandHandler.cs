using MediatR;
using TaskManagementSystem.SharedLib.Exceptions;
using TaskManagementSystem.TaskBoardService.Core.Interfaces.Repository;

namespace TaskManagementSystem.TaskBoardService.Application.Commands.Handlers;


public class DeleteBoardCommandHandler : IRequestHandler<DeleteBoardCommand>
{
    private readonly ITaskBoardRepository _repository;

    public DeleteBoardCommandHandler(ITaskBoardRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteBoardCommand request, CancellationToken cancellationToken)
    {
        var board = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (board == null)
        {
            throw AppException.NotFound();
        }

        board.Delete();

        await _repository.DeleteAsync(request.Id, cancellationToken);
    }
}

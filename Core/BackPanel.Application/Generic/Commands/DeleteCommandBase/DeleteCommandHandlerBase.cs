using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using MediatR;

namespace BackPanel.Application.Generic.Commands.DeleteCommandBase
{
    public class DeleteCommandHandlerBase<TEntity, TCommand> : IRequestHandler<TCommand>
        where TEntity : EntityBase
        where TCommand : DeleteCommandBase<TEntity>
    {
        private readonly IRepositoryBase<TEntity> _repository;
        public DeleteCommandHandlerBase(IRepositoryBase<TEntity> repository)
        {
            _repository = repository;
        }
        public virtual async Task Handle(TCommand request, CancellationToken cancellationToken)
        {
            await _repository.DeleteAsync(request.Id);
            await _repository.Complete();
        }
    }

}

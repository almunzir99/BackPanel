using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using MediatR;

namespace BackPanel.Application.Generic.Commands.DeleteCommandBase
{
    public class DeleteCommandHandlerBase<TEntity, TCommand> : IRequestHandler<TCommand>
        where TEntity : EntityBase
        where TCommand : DeleteCommandBase<TEntity>
    {
        protected readonly IRepositoryBase<TEntity> Repository;
        public DeleteCommandHandlerBase(IRepositoryBase<TEntity> repository)
        {
            Repository = repository;
        }
        public virtual async Task Handle(TCommand request, CancellationToken cancellationToken)
        {
            await Repository.DeleteAsync(request.Id);
            await Repository.Complete();
        }
    }

}

using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using BackPanel.Domain.Enums;
using MediatR;

namespace BackPanel.Application.Generic.Common.Commands.Handlers
{
    public abstract class ToggleActiveCommandHandlerBase<TEntity, TCommand> : IRequestHandler<TCommand>
        where TEntity : EntityBase
        where TCommand : ToggleActiveCommandBase<TEntity>
    {
        protected readonly IRepositoryBase<TEntity> Repository;
        protected ToggleActiveCommandHandlerBase(IRepositoryBase<TEntity> repository)
        {
            Repository = repository;
        }
        public async Task Handle(TCommand request, CancellationToken cancellationToken)
        {
            var entity = await Repository.GetById(request.Id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Entity with ID {request.Id} not found.");
            }
            entity.Status = entity.Status == Status.Active ? Status.Disabled : Status.Active;
            await Repository.Complete(); ;
        }

    }
}

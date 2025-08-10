using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using BackPanel.Domain.Enums;
using MediatR;

namespace BackPanel.Application.Generic.Commands.ToggleActiveCommandBase
{
    public class ToggleActiveCommandBaseHandler<TEntity, TCommand> : IRequestHandler<TCommand>
        where TEntity : EntityBase
        where TCommand : ToggleActiveCommandBase<TEntity>
    {
        protected readonly IRepositoryBase<TEntity> _repository;
        protected ToggleActiveCommandBaseHandler(IRepositoryBase<TEntity> repository)
        {
            _repository = repository;
        }
        public async Task Handle(TCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.SingleAsync(request.Id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Entity with ID {request.Id} not found.");
            }
            entity.Status = entity.Status == Status.Active ? Status.Disabled : Status.Active;
            await _repository.Complete(); ;
        }

    }
}

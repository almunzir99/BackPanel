using AutoMapper;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using MediatR;

namespace BackPanel.Application.Generic.Common.Commands.Handlers
{
    public abstract class CreateCommandHandlerBase<TEntity, TDTORequest, TDTO, TCommand> : IRequestHandler<TCommand, TDTO>
        where TEntity : EntityBase
        where TDTO : class
        where TCommand : CreateCommandBase<TDTORequest, TDTO>
    {
        protected readonly IRepositoryBase<TEntity> Repository;
        protected readonly IMapper Mapper;
        public CreateCommandHandlerBase(IRepositoryBase<TEntity> repository, IMapper mapper)
        {
            Repository = repository;
            Mapper = mapper;
        }
        public virtual async Task<TDTO> Handle(TCommand request, CancellationToken cancellationToken)
        {
            var mappedItem = Mapper.Map<TEntity>(request.Request);
            await Repository.CreateAsync(mappedItem);
            await Repository.Complete();
            var result = Mapper.Map<TDTO>(mappedItem);
            return result;
        }

    }
}

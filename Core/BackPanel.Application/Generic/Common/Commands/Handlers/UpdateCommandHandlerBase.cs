using AutoMapper;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using MediatR;

namespace BackPanel.Application.Generic.Common.Commands.Handlers
{
    public abstract class UpdateCommandHandlerBase<TEntity, TDTORequest, TDTO, TCommand> : IRequestHandler<TCommand, TDTO>
       where TEntity : EntityBase
       where TDTO : class
       where TCommand : UpdateCommandBase<TDTORequest, TDTO>
    {
        protected readonly IRepositoryBase<TEntity> Repository;
        protected readonly IMapper Mapper;
        public UpdateCommandHandlerBase(IRepositoryBase<TEntity> repository, IMapper mapper)
        {
            Repository = repository;
            Mapper = mapper;
        }
        public virtual async Task<TDTO> Handle(TCommand request, CancellationToken cancellationToken)
        {
            var id = request.Id;
            var mappedItem = Mapper.Map<TDTORequest, TEntity>(request.Request);
            mappedItem.Id = id;
            var result = await Repository.UpdateAsync( mappedItem);
            await Repository.Complete();
            return Mapper.Map<TEntity, TDTO>(result);
        }

    }

}
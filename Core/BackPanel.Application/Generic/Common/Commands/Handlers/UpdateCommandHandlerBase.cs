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
            var entity = await Repository.GetById(request.Id);
            Mapper.Map(request.Request, entity);
            var result = await Repository.UpdateAsync(entity);
            await Repository.Complete();
            return Mapper.Map<TEntity, TDTO>(result);
        }

    }

}
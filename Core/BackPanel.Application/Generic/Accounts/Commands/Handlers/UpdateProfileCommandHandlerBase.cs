using AutoMapper;
using BackPanel.Application.DTOs;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using MediatR;

namespace BackPanel.Application.Generic.Accounts.Commands.Handlers
{
    public abstract class UpdateProfileCommandHandlerBase<TEntity, TDTORequest, TDTO, TCommand> : IRequestHandler<TCommand, TDTO>
        where TEntity : UserEntityBase
        where TDTORequest : class
        where TDTO : UserDtoBase
        where TCommand : UpdateProfileCommandBase<TDTORequest, TDTO>
    {
        protected readonly IRepositoryBase<TEntity> Repository;
        protected readonly IMapper Mapper;
        protected UpdateProfileCommandHandlerBase(IRepositoryBase<TEntity> repository, IMapper mapper)
        {
            Repository = repository;
            Mapper = mapper;
        }
        public virtual async Task<TDTO> Handle(TCommand request, CancellationToken cancellationToken)
        {
            var entity = await Repository.GetById(request.Id);
            if (entity == null)
                throw new Exception("User not found");
            Mapper.Map(request.Request, entity);
            entity.LastUpdate = DateTime.Now;
            await Repository.UpdateAsync(  entity);
            await Repository.Complete();
            return Mapper.Map<TEntity, TDTO>(entity);
        }
    }
}

using AutoMapper;
using BackPanel.Application.DTOs;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using MediatR;

namespace BackPanel.Application.Generic.Accounts.Commands.UpdateProfileBase
{
    public abstract class UpdateProfileCommandBaseHandlerBase<TEntity, TDTORequest, TDTO> : IRequestHandler<UpdateProfileCommandBase<TDTORequest, TDTO>, TDTO>
        where TEntity : UserEntityBase
        where TDTORequest : class
        where TDTO : UserDtoBase
    {
        protected readonly IRepositoryBase<TEntity> Repository;
        protected readonly IMapper Mapper;
        protected UpdateProfileCommandBaseHandlerBase(IRepositoryBase<TEntity> repository, IMapper mapper)
        {
            Repository = repository;
            Mapper = mapper;
        }
        public virtual async Task<TDTO> Handle(UpdateProfileCommandBase<TDTORequest, TDTO> request, CancellationToken cancellationToken)
        {
            var entity = await Repository.SingleAsync(request.Id);
            if (entity == null)
                throw new Exception("User not found");
            Mapper.Map(request.Request, entity);
            entity.LastUpdate = DateTime.Now;
            await Repository.UpdateAsync(request.Id, entity);
            await Repository.Complete();
            return Mapper.Map<TEntity, TDTO>(entity);
        }
    }
}

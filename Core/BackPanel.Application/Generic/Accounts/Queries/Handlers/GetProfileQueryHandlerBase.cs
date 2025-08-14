using AutoMapper;
using BackPanel.Application.DTOs;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using MediatR;

namespace BackPanel.Application.Generic.Accounts.Queries.Handlers
{
    public abstract class GetProfileQueryHandlerBase<TEntity, TDTO, TQuery> : IRequestHandler<TQuery, TDTO>
        where TDTO : UserDtoBase
        where TEntity : UserEntityBase
        where TQuery : GetProfileQueryBase<TDTO>
    {
        protected readonly IRepositoryBase<TEntity> Repository;
        protected readonly IMapper Mapper;

        protected GetProfileQueryHandlerBase(IRepositoryBase<TEntity> repository, IMapper mapper)
        {
            Repository = repository;
            Mapper = mapper;
        }

        public async Task<TDTO> Handle(TQuery request, CancellationToken cancellationToken)
        {
            var target = await Repository.SingleAsync(x => x.Id == request.Id);
            return Mapper.Map<TDTO>(target);
        }
    }

}
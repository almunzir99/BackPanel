using AutoMapper;
using BackPanel.Application.DTOs;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using MediatR;

namespace BackPanel.Application.Generic.Accounts.Queries.GetProfileBase
{
    public abstract class GetProfileQueryHandlerBase<TEntity, TDTO> : IRequestHandler<GetProfileQueryBase<TDTO>, TDTO>
        where TDTO : UserDtoBase
        where TEntity : UserEntityBase
    {
        protected readonly IRepositoryBase<TEntity> Repository;
        protected readonly IMapper Mapper;

        protected GetProfileQueryHandlerBase(IRepositoryBase<TEntity> repository, IMapper mapper)
        {
            Repository = repository;
            Mapper = mapper;
        }

        public async Task<TDTO> Handle(GetProfileQueryBase<TDTO> request, CancellationToken cancellationToken)
        {
            var target = await Repository.SingleAsync(x => x.Id == request.Id);
            return Mapper.Map<TDTO>(target);
        }
    }

}
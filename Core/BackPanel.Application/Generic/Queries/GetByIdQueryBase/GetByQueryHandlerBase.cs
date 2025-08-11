using AutoMapper;
using BackPanel.Application.DTOs;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using MediatR;

namespace BackPanel.Application.Generic.Queries.GetByIdQueryBase
{
    public class GetByQueryHandlerBase<TDTO, TEntity> : IRequestHandler<GetByIdQueryBase<TDTO>, TDTO>
        where TDTO : DtoBase
        where TEntity : EntityBase
    {
        protected readonly IRepositoryBase<TEntity> Repository;
        protected readonly IMapper Mapper;
        public GetByQueryHandlerBase(IRepositoryBase<TEntity> repository, IMapper mapper)
        {
            Repository = repository;
            Mapper = mapper;
        }
        public async Task<TDTO> Handle(GetByIdQueryBase<TDTO> request, CancellationToken cancellationToken)
        {
            var entity = await Repository.SingleAsync(request.Id);
            return Mapper.Map<TDTO>(entity);
        }
    }

}


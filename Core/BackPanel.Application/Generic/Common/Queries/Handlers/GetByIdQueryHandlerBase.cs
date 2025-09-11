using AutoMapper;
using BackPanel.Application.DTOs;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using MediatR;

namespace BackPanel.Application.Generic.Common.Queries.Handlers
{
    public abstract class GetByIdQueryHandlerBase<TEntity, TDTO, TQuery> : IRequestHandler<TQuery, TDTO>
        where TDTO : DtoBase
        where TEntity : EntityBase
        where TQuery : GetByIdQueryBase<TDTO>
    {
        protected readonly IRepositoryBase<TEntity> Repository;
        protected readonly IMapper Mapper;
        public GetByIdQueryHandlerBase(IRepositoryBase<TEntity> repository, IMapper mapper)
        {
            Repository = repository;
            Mapper = mapper;
        }
        public async Task<TDTO> Handle(TQuery request, CancellationToken cancellationToken)
        {
            var entity = await Repository.GetById(request.Id);
            return Mapper.Map<TDTO>(entity);
        }
    }

}


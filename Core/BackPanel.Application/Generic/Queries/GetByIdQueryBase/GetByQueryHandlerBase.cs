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
        private readonly IRepositoryBase<TEntity> _repository;
        private readonly IMapper _mapper;
        public GetByQueryHandlerBase(IRepositoryBase<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<TDTO> Handle(GetByIdQueryBase<TDTO> request, CancellationToken cancellationToken)
        {
            var entity = await _repository.SingleAsync(request.Id);
            return _mapper.Map<TDTO>(entity);
        }
    }

}


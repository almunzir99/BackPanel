using AutoMapper;
using BackPanel.Application.DTOs;
using BackPanel.Application.Features.Business.Queries;
using BackPanel.Application.Interfaces;
using MediatR;

namespace BackPanel.Application.Features.Business.Queries.Handlers
{
    public class GetBusinessQueryHandler : IRequestHandler<GetBusinessQuery, BusinessDto>
    {
        private readonly IRepositoryBase<Domain.Entities.Business> _repository;
        private readonly IMapper _mapper;

        public GetBusinessQueryHandler(IRepositoryBase<Domain.Entities.Business> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<BusinessDto> Handle(GetBusinessQuery request, CancellationToken cancellationToken)
        {
            var business = await _repository.FirstOrDefaultAsync();
            return _mapper.Map<BusinessDto>(business);
        }
    }
}

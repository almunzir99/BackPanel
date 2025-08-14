using AutoMapper;
using BackPanel.Application.DTOs;
using BackPanel.Application.Features.CompanyInfo.Queries;
using BackPanel.Application.Interfaces;
using MediatR;

namespace BackPanel.Application.Features.CompanyInfo.Queries.Handlers
{
    public class GetCompanyInfoQueryHandler : IRequestHandler<GetCompanyInfoQuery, CompanyInfoDto>
    {
        private readonly IRepositoryBase<Domain.Entities.CompanyInfo> _repository;
        private readonly IMapper _mapper;

        public GetCompanyInfoQueryHandler(IRepositoryBase<Domain.Entities.CompanyInfo> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CompanyInfoDto> Handle(GetCompanyInfoQuery request, CancellationToken cancellationToken)
        {
            var companyInfo = await _repository.FirstOrDefaultAsync();
            return _mapper.Map<CompanyInfoDto>(companyInfo);
        }
    }
}

using BackPanel.Application.DTOs;
using MediatR;

namespace BackPanel.Application.Features.CompanyInfo.Queries.GetCompanyInfo
{
    public record GetCompanyInfoQuery : IRequest<CompanyInfoDto>;
}

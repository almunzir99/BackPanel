using BackPanel.Application.DTOs;
using MediatR;

namespace BackPanel.Application.Features.CompanyInfo.Queries
{
    public record GetCompanyInfoQuery : IRequest<CompanyInfoDto>;
}

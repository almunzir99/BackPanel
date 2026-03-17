using BackPanel.Application.DTOs;
using MediatR;

namespace BackPanel.Application.Features.Business.Queries
{
    public record GetBusinessQuery : IRequest<BusinessDto>;
}

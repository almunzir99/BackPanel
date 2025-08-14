using AutoMapper;
using BackPanel.Application.DTOs;
using BackPanel.Application.Generic.Accounts.Queries;
using BackPanel.Application.Generic.Accounts.Queries.Handlers;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;

namespace BackPanel.Application.Features.Admins.Queries.Handlers
{
    public class GetProfileAdminQueryHandler : GetProfileQueryHandlerBase<Admin, AdminDto, GetProfileQueryBase<AdminDto>>
    {
        public GetProfileAdminQueryHandler(IRepositoryBase<Admin> repository, IMapper mapper) : base(repository, mapper)
        {
        }
    }
}

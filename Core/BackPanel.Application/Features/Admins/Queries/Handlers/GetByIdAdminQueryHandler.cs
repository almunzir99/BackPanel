using AutoMapper;
using BackPanel.Application.DTOs;
using BackPanel.Application.Generic.Common.Queries;
using BackPanel.Application.Generic.Common.Queries.Handlers;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;

namespace BackPanel.Application.Features.Admins.Queries.Handlers
{
    public class GetByIdAdminQueryHandler : GetByIdQueryHandlerBase<Admin, AdminDto, GetByIdQueryBase<AdminDto>>
    {
        public GetByIdAdminQueryHandler(IRepositoryBase<Admin> repository, IMapper mapper) : base(repository, mapper)
        {
            repository.PrepareDbSet(x => x.Role!);
        }
    }
}

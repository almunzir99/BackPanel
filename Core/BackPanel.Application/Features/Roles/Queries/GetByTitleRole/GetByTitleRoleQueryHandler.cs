using AutoMapper;
using BackPanel.Application.DTOs;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using MediatR;

namespace BackPanel.Application.Features.Roles.Queries.GetByTitleRole
{
    public class GetByTitleRoleQueryHandler : IRequestHandler<GetByTitleRoleQuery, RoleDto>
    {
        private readonly IRepositoryBase<Role> _repository;
        private readonly IMapper _mapper;
        public GetByTitleRoleQueryHandler(IRepositoryBase<Role> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<RoleDto> Handle(GetByTitleRoleQuery request, CancellationToken cancellationToken)
        {
            var role = await _repository.SingleAsync(c => c.Title == request.Title);
            if (role == null)
            {
                throw new KeyNotFoundException($"Role with title '{request.Title}' not found.");
            }
            return _mapper.Map<RoleDto>(role);
        }
    }
}

using BackPanel.Application.Generic.Common.Commands;
using BackPanel.Application.Generic.Common.Commands.Handlers;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;

namespace BackPanel.Application.Features.Roles.Commands.Handlers;
public class ToggleActiveRoleCommandHander : ToggleActiveCommandHandlerBase<Role, ToggleActiveCommandBase<Role>>
{
    public ToggleActiveRoleCommandHander(IRepositoryBase<Role> repository) : base(repository)
    {
    }
}
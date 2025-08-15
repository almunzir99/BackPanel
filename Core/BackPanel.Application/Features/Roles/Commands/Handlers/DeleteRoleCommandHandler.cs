using BackPanel.Application.Generic.Common.Commands;
using BackPanel.Application.Generic.Common.Commands.Handlers;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;

namespace BackPanel.Application.Features.Roles.Commands.Handlers;
public class DeleteRoleCommandHander : DeleteCommandHandlerBase<Role, DeleteCommandBase<Role>>
{
    public DeleteRoleCommandHander(IRepositoryBase<Role> repository) : base(repository)
    {
    }
}
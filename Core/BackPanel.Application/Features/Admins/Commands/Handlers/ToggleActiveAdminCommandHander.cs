using BackPanel.Application.Generic.Common.Commands;
using BackPanel.Application.Generic.Common.Commands.Handlers;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;

namespace BackPanel.Application.Features.Admins.Commands.Handlers
{
    public class ToggleActiveAdminCommandHander : ToggleActiveCommandHandlerBase<Admin, ToggleActiveCommandBase<Admin>>
    {
        public ToggleActiveAdminCommandHander(IRepositoryBase<Admin> repository) : base(repository)
        {
        }
    }
}

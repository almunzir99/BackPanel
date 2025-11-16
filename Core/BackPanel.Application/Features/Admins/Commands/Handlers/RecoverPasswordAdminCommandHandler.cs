using BackPanel.Application.Generic.Accounts.Commands.Handlers;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;

namespace BackPanel.Application.Features.Admins.Commands.Handlers
{
    public class RecoverPasswordAdminCommandHandler : RecoverPasswordCommandHandlerBase<Admin>
    {
        public RecoverPasswordAdminCommandHandler(IRepositoryBase<Admin> repository) : base(repository)
        {
        }
    }
}

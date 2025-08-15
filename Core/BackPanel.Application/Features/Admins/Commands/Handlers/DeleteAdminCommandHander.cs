using BackPanel.Application.Generic.Common.Commands;
using BackPanel.Application.Generic.Common.Commands.Handlers;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;

namespace BackPanel.Application.Features.Admins.Commands.Handlers
{
    public class DeleteAdminCommandHander : DeleteCommandHandlerBase<Admin, DeleteCommandBase<Admin>>
    {
        public DeleteAdminCommandHander(IRepositoryBase<Admin> repository) : base(repository)
        {
        }
    }
}

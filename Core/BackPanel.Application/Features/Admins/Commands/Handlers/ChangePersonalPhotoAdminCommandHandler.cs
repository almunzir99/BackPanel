using BackPanel.Application.Generic.Accounts.Commands;
using BackPanel.Application.Generic.Accounts.Commands.Handlers;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using BackPanel.FilesManager.Interfaces;

namespace BackPanel.Application.Features.Admins.Commands.Handlers
{
    public class ChangePersonalPhotoAdminCommandHandler : ChangePersonalPhotoCommandHandlerBase<Admin, ChangePersonalPhotoCommandBase<Admin>>
    {
        public ChangePersonalPhotoAdminCommandHandler(IFilesManagerService filesManager, IRepositoryBase<Admin> repository) : base(filesManager, repository)
        {
        }
    }
}

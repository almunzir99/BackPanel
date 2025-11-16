using BackPanel.Application.Generic.Accounts.Commands.Handlers;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace BackPanel.Application.Features.Admins.Commands.Handlers
{
    public class ValidateCodeAdminCommandHandler : ValidateCodeCommandHandlerBase<Admin>
    {
        public ValidateCodeAdminCommandHandler(IRepositoryBase<Admin> repository, IMemoryCache memoryCache) : base(repository, memoryCache)
        {
        }
    }

}

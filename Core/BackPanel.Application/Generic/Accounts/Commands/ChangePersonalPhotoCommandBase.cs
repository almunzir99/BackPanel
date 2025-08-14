using BackPanel.FilesManager.Interfaces;
using BackPanel.FilesManager.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackPanel.Application.Generic.Accounts.Commands
{
    public record ChangePersonalPhotoCommandBase<TEntity>(int Id, IWebFormFile File) : IRequest<string>;

}

using BackPanel.FilesManager.Interfaces;
using BackPanel.FilesManager.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackPanel.Application.Generic.Authentication.Commands.ChangePersonalPhotoBase
{
    public record ChangePersonalPhotoCommandBase<TEntity>(int Id, IWebFormFile File) : IRequest<string>;

}

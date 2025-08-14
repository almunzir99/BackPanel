using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using BackPanel.FilesManager.Interfaces;
using MediatR;

namespace BackPanel.Application.Generic.Accounts.Commands.Handlers
{
    public abstract class ChangePersonalPhotoCommandHandlerBase<TEntity, TCommand>
        : IRequestHandler<TCommand, string>
        where TEntity : UserEntityBase
        where TCommand : ChangePersonalPhotoCommandBase<TEntity>
    {
        protected readonly IFilesManagerService FileManagerService;
        protected readonly IRepositoryBase<TEntity> Repository;
        public ChangePersonalPhotoCommandHandlerBase(IFilesManagerService filesManager, IRepositoryBase<TEntity> repository)
        {
            FileManagerService = filesManager;
            Repository = repository;
        }
        public async Task<string> Handle(TCommand request, CancellationToken cancellationToken)
        {
            var user = await Repository.SingleAsync(c => c.Id == request.Id);
            if (user == null)
                throw new Exception("this user isn't available");
            var oldPhoto = user.Image;
            var result = await FileManagerService.UploadSingleFile("assets/images/users", request.File);
            user.Image = result.Path.Replace("//", "/");
            await Repository.Complete();
            if (oldPhoto != null && FileManagerService.FileExists(oldPhoto))
                FileManagerService.DeleteFile(oldPhoto, "");
            return user.Image;
        }
    }

}

using BackPanel.Application.Helpers;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using MediatR;

namespace BackPanel.Application.Generic.Accounts.Commands.Handlers
{
    public abstract class PasswordResetCommandHandlerBase<TEntity, TCommand> : IRequestHandler<TCommand, bool>
        where TEntity : UserEntityBase
        where TCommand : PasswordResetCommandBase<TEntity>
    {
        protected readonly IRepositoryBase<TEntity> Repository;
        public PasswordResetCommandHandlerBase(IRepositoryBase<TEntity> repository)
        {
            Repository = repository;
        }
        public async Task<bool> Handle(TCommand request, CancellationToken cancellationToken)
        {
            var user = await Repository.FindAsync(c => c.Id == request.Id);
            if (user == null)
                throw new Exception("this user isn't available");
            var validOldPassword = user.PasswordSalt != null && user.PasswordHash != null &&
                                   HashingHelper.VerifyPassword(request.Request.OldPassword, user.PasswordHash, user.PasswordSalt);
            if (!validOldPassword)
                throw new Exception("invalid old password");
            HashingHelper.CreateHashPassword(request.Request.NewPassword, out var pHash, out var pSalt);
            user.PasswordHash = pHash;
            user.PasswordSalt = pSalt;
            await Repository.Complete();
            return true;
        }
    }
}
using BackPanel.Application.Helpers;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using MediatR;

namespace BackPanel.Application.Generic.Accounts.Commands.Handlers
{
    public class RecoverPasswordCommandHandlerBase<TEntity> : IRequestHandler<RecoverPasswordCommandBase<TEntity>>
           where TEntity : UserEntityBase
    {
        private readonly IRepositoryBase<TEntity> _repository;

        public RecoverPasswordCommandHandlerBase(IRepositoryBase<TEntity> repository)
        {
            _repository = repository;
        }

        public async Task Handle(RecoverPasswordCommandBase<TEntity> request, CancellationToken cancellationToken)
        {
            var user = await _repository.GetById(request.userId);
            if (user == null)
                throw new Exception("invalid user email");
            byte[] pHash, pSalt;
            HashingHelper.CreateHashPassword(request.newPassword, out pHash, out pSalt);
            user.PasswordHash = pHash;
            user.PasswordSalt = pSalt;
            await _repository.Complete();
        }
    }
}

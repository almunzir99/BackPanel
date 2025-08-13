using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using MediatR;

namespace BackPanel.Application.Generic.Authentication.Commands.PasswordRecoverBase
{
    public abstract class PasswordRecoveryCommandHandlerBase<TEntity> : IRequestHandler<PasswordRecoverCommandBase<TEntity>, bool>
        where TEntity : UserEntityBase
    {
        protected readonly IRepositoryBase<TEntity> Repository;
        public PasswordRecoveryCommandHandlerBase(IRepositoryBase<TEntity> repository)
        {
            Repository = repository;
        }
        public virtual async Task<bool> Handle(PasswordRecoverCommandBase<TEntity> request, CancellationToken cancellationToken)
        {
            var recoveryRequest = request.Request;
            // Implement the logic to handle password recovery
            // This could involve sending an email with a reset link, etc.
            // For now, we will just return true to indicate success.
            return await Task.FromResult(true);
        }
    }
}

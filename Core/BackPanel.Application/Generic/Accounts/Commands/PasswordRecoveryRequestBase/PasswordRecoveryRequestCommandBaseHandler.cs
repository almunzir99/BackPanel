using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using MediatR;

namespace BackPanel.Application.Generic.Authentication.Commands.PasswordRecoveryRequestBase
{
    public abstract class PasswordRecoveryRequestCommandBaseHandler<TEntity> : IRequestHandler<PasswordRecoveryRequestCommandBase<TEntity>, bool>
        where TEntity : UserEntityBase
    {
        protected readonly IRepositoryBase<TEntity> Repository;
        public PasswordRecoveryRequestCommandBaseHandler(IRepositoryBase<TEntity> repository)
        {
            Repository = repository;
        }
        public async Task<bool> Handle(PasswordRecoveryRequestCommandBase<TEntity> request, CancellationToken cancellationToken)
        {
            var user = await Repository.SingleAsync(c => c.Email == request.Email);
            if (user == null)
                throw new Exception("This email is not registered");
            // Logic to send password recovery email goes here
            return true; // Indicating that the request was successful
        }
    }
}

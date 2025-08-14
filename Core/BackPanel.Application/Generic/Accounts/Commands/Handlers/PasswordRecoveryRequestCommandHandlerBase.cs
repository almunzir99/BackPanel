using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using MediatR;

namespace BackPanel.Application.Generic.Accounts.Commands.Handlers
{
    public abstract class PasswordRecoveryRequestCommandHandlerBase<TEntity, TCommand> : IRequestHandler<TCommand, bool>
        where TEntity : UserEntityBase
        where TCommand : PasswordRecoveryRequestCommandBase<TEntity>
    {
        protected readonly IRepositoryBase<TEntity> Repository;
        public PasswordRecoveryRequestCommandHandlerBase(IRepositoryBase<TEntity> repository)
        {
            Repository = repository;
        }
        public async Task<bool> Handle(TCommand request, CancellationToken cancellationToken)
        {
            var user = await Repository.SingleAsync(c => c.Email == request.Email);
            if (user == null)
                throw new Exception("This email is not registered");
            // Logic to send password recovery email goes here
            return true; // Indicating that the request was successful
        }
    }
}

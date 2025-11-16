using BackPanel.Application.DTOsRequests;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace BackPanel.Application.Generic.Accounts.Commands.Handlers
{
    public class ValidateCodeCommandHandlerBase<TEntity> : IRequestHandler<ValidateCodeCommandBase<TEntity>, EmailRecoveryRequest>
        where TEntity : UserEntityBase
    {
        private readonly IRepositoryBase<TEntity> _repository;
        private readonly IMemoryCache _memoryCache;

        public ValidateCodeCommandHandlerBase(IRepositoryBase<TEntity> repository, IMemoryCache memoryCache)
        {
            _repository = repository;
            _memoryCache = memoryCache;
        }

        public async Task<EmailRecoveryRequest> Handle(ValidateCodeCommandBase<TEntity> request, CancellationToken cancellationToken)
        {
            var user = await _repository.FindAsync(c => c.Email!.ToLower() == request.Email.ToLower());
            if (user == null)
                throw new Exception("invalid user email");
            var emailRequest = _memoryCache.Get<EmailRecoveryRequest>(request.IsEmailValidation ? $"vd_{user.Email}" : $"pr_{user.Email}");
            if (emailRequest == null)
                throw new Exception("invalid request");
            if (emailRequest.Code != request.Code)
                throw new Exception("invalid code");
            if (emailRequest.ExpireAt < DateTime.Now)
                throw new Exception("this code is expired");
            return emailRequest;
        }
    }
}

using AutoMapper;
using BackPanel.Application.DTOs;
using BackPanel.Application.Helpers;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using BackPanel.Domain.Enums;
using MediatR;

namespace BackPanel.Application.Generic.Accounts.Commands.Handlers
{
    public abstract class AuthenticateCommandHandlerBase<TEntity, TDTO> : IRequestHandler<AuthenticateCommandBase<TEntity, TDTO>, TDTO>
        where TEntity : UserEntityBase
        where TDTO : UserDtoBase
    {
        protected readonly IRepositoryBase<TEntity> Repository;
        protected readonly IMapper Mapper;
        private readonly IWebConfiguration _webConfiguration;
        protected abstract string UserType { get; }

        protected AuthenticateCommandHandlerBase(IRepositoryBase<TEntity> repository, IMapper mapper, IWebConfiguration webConfiguration)
        {
            Repository = repository;
            Mapper = mapper;
            _webConfiguration = webConfiguration;
        }
        public virtual async Task<TDTO> Handle(AuthenticateCommandBase<TEntity, TDTO> request, CancellationToken cancellationToken)
        {
            var model = request.Model;
            // Get User By Email
            var user = await Repository.SingleAsync(c => c.Email == model.Email);
            if (user == null)
                throw new Exception("This account isn't available");
            if (user.Status == Status.Disabled)
                throw new Exception("this account is locked please contact the administrator");
            //verify the password
            var verified = user.PasswordSalt != null && user.PasswordHash != null
                                                     && HashingHelper.VerifyPassword(model.Password!, user.PasswordHash,
                                                         user.PasswordSalt);
            if (!verified)
                throw new Exception("The password isn't correct");
            var mappedUser = Mapper.Map<TEntity, TDTO>(user);
            //Generate Token
            var role = mappedUser is AdminDto admin ? admin.Role : null;
            var secretKey = _webConfiguration.GetSecretKey();
            var token = JwtHelper.GenerateToken(mappedUser, UserType, secretKey, role);
            mappedUser.Token = token;
            return mappedUser;
        }
    }
}

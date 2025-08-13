using AutoMapper;
using BackPanel.Application.DTOs;
using BackPanel.Application.DTOsRequests;
using BackPanel.Application.Helpers;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using MediatR;

namespace BackPanel.Application.Generic.Authentication.Commands.RegisterCommandBase
{
    public abstract class RegisterCommandBaseHandlerBase<TEntity, TDTORequest, TDTO,TCommand> : IRequestHandler<RegisterCommandBase<TDTORequest, TDTO>, TDTO>
        where TEntity : UserEntityBase
        where TDTORequest : UserBaseDtoRequest
        where TDTO : UserDtoBase
    {
        protected readonly IRepositoryBase<TEntity> Repository;
        protected readonly IMapper Mapper;
        private readonly IWebConfiguration _webConfiguration;
        protected abstract string UserType { get; }

        protected RegisterCommandBaseHandlerBase(IRepositoryBase<TEntity> repository, IMapper mapper, IWebConfiguration webConfiguration)
        {
            Repository = repository;
            Mapper = mapper;
            _webConfiguration = webConfiguration;
        }
        public virtual async Task<TDTO> Handle(RegisterCommandBase<TDTORequest, TDTO> request, CancellationToken cancellationToken)
        {
            var user = request.Model;
            if (user.Password == null)
                throw new Exception("password shouldn't be null");
            var mappedUser = Mapper.Map<TDTORequest, TEntity>(user);
            mappedUser.CreatedAt = DateTime.Now;
            mappedUser.LastUpdate = DateTime.Now;
            HashingHelper.CreateHashPassword(user.Password!, out var pHash, out var pSalt);
            mappedUser.PasswordHash = pHash;
            mappedUser.PasswordSalt = pSalt;
            await Repository.CreateAsync(mappedUser);
            await Repository.Complete();
            // Generate Token
            var secretKey = _webConfiguration.GetSecretKey();
            var role = (mappedUser is Admin admin) ? admin.Role : null;
            var result = Mapper.Map<TEntity, TDTO>(mappedUser);
            if (role != null)
            {
                var mappedRole = Mapper.Map<Role, RoleDto>(role);
                var token = JwtHelper.GenerateToken(result, UserType, secretKey, mappedRole);
                result.Token = token;
            }

            return result;
        }
    }
}

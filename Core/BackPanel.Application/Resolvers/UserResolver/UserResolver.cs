using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackPanel.Application.Resolvers.UserResolver
{

    public class UserResolver : IUserResolver
    {
        private readonly IRepositoryBase<Admin> _adminRepository;
        public UserResolver(IRepositoryBase<Admin> adminRepository)
        {
            _adminRepository = adminRepository;
            _adminRepository.IncludeableDbSet = _adminRepository.IncludeableDbSet.Include(c => c.Notifications);
        }

        public async Task<UserEntityBase> GetUserAsync(int userId, string userType)
        {
            UserEntityBase user = userType.ToLowerInvariant() switch
            {
                "admin" => await _adminRepository.SingleAsync(userId),
                _ => throw new ArgumentException($"Unsupported user type: {userType}", nameof(userType))
            };

            return user ?? throw new Exception($"User with ID {userId} and type {userType} not found");
        }

        public async Task<IList<UserEntityBase>> GetUsersByTypeAsync(string userType)
        {
            return userType.ToLowerInvariant() switch
            {
                "admin" => (await _adminRepository.ListAsync()).Cast<UserEntityBase>().ToList(),
                _ => throw new ArgumentException($"Unsupported user type: {userType}", nameof(userType))
            };
        }
    }

}

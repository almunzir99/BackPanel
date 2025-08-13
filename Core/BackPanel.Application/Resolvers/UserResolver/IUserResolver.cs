using BackPanel.Domain.Entities;

namespace BackPanel.Application.Resolvers.UserResolver
{
    public interface IUserResolver
    {
        Task<UserEntityBase> GetUserAsync(int userId, string userType);
        Task<IList<UserEntityBase>> GetUsersByTypeAsync(string userType);
    }

}

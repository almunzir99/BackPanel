using BackPanel.Application.DTOs;
using BackPanel.Application.DTOsRequests;
using BackPanel.Application.DTOs.Filters;
using BackPanel.Application.DTOs.Wrapper;

namespace BackPanel.Application.Interfaces;

public interface IUserService
{
    Task<AppUserDto> AuthenticateAsync(AuthenticationModel model);
    Task<AppUserDto> RegisterAsync(AppUserDtoRequest request);
    Task<AppUserDto> GetProfileAsync(int userId);
    Task<AppUserDto> UpdateProfileAsync(int userId, AppUserDtoRequest request);
    Task<string> UpdatePhotoAsync(int userId, string imagePath);
    Task<bool> RequestPasswordRecoveryAsync(string email);
    Task<EmailRecoveryRequest> ValidatePasswordRecoveryCodeAsync(string email, int code);
    Task RecoverPasswordAsync(int userId, string newPassword);
    Task<bool> ResetPasswordAsync(int userId, string oldPassword, string newPassword);
    Task<IList<AppUserDto>> GetAllAsync();
    Task<Tuple<List<AppUserDto>, int>> GetAllFilteredAsync(ListFilter filter);
    Task<byte[]> ExportToExcelAsync();
    Task<AppUserDto> GetByIdAsync(int userId);
    Task DeleteAsync(int userId);
    Task ToggleActiveAsync(int userId);
    Task<IList<int>> GetAllUserIdsAsync();
}

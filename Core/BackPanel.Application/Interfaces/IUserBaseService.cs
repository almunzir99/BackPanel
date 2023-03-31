using BackPanel.Application.DTOs;
using BackPanel.Application.DTOsRequests;
using BackPanel.Domain.Entities;
using BackPanel.FilesManager.Interfaces;
using Microsoft.AspNetCore.JsonPatch;

namespace BackPanel.Application.Interfaces;

public interface IUserBaseService<TEntity,TDto,  TDtoRequest> : IServicesBase<TEntity,TDto,TDtoRequest>
where TEntity: UserEntityBase
where  TDto : UserDtoBase
{
    Task<TDto> Authenticate(AuthenticationModel model);
    Task<TDto> Register(TDtoRequest user);
    Task PasswordRecoveryRequest(string email);
    Task PasswordRecovery(string key,string newPassword);
        
    Task ResetPassword(int id,string oldPassword,string newPassword);
    Task<string> ChangePersonalPhoto(int id,IWebFormFile file);
    Task<TDto> UpdatePersonalInfo(int userId,JsonPatchDocument<TEntity> patchDoc);
    Task<TDto> GetProfileAsync(int userId);  
}
using AutoMapper;
using BackPanel.Application.DTOs;
using BackPanel.Application.DTOsRequests;
using BackPanel.Application.Helpers;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using BackPanel.Domain.Enums;
using BackPanel.FilesManager.Interfaces;
using BackPanel.SMTP.Interfaces;
using Microsoft.AspNetCore.JsonPatch;

namespace BackPanel.Application.Services;

public abstract class UserBaseService<TEntity, TDto, TDtoRequest> : ServiceBase<TEntity, TDto, TDtoRequest>,
    IUserBaseService<TEntity, TDto, TDtoRequest>
    where TEntity : UserEntityBase
    where TDto : UserDtoBase
    where TDtoRequest : UserBaseDtoRequest
{
    protected abstract string UserType { get; }
    private readonly ISmtpService _smtpService;
    private readonly IFilesManagerService _filesManagerService;
    private readonly IWebConfiguration _webConfiguration;

    protected UserBaseService(IMapper mapper,
        ISmtpService smtpService,
        IFilesManagerService filesManagerService,
        IRepositoryBase<TEntity> repository,
        IRepositoryBase<Admin> adminsRepository, IWebConfiguration webConfiguration,IPathProvider pathProvider)
        : base(mapper, repository, adminsRepository,pathProvider)
    {
        _smtpService = smtpService;
        _filesManagerService = filesManagerService;
        _webConfiguration = webConfiguration;
    }

    public override async Task<TDto> SingleAsync(int id)
    {
        var result = await base.SingleAsync(id);
        return result;
    }

    public virtual async Task<TDto> Authenticate(AuthenticationModel model)
    {
        // Get User By Email
        var user = await Repository.SingleAsync(c => c.Email == model.Email);
        if (user == null)
            throw new Exception("This account isn't available");
        if(user.Status == Status.Disabled)
            throw new Exception("this account is locked please contact the administrator");
        //verify the password
        var verified = user.PasswordSalt != null && user.PasswordHash != null
                                                 && HashingHelper.VerifyPassword(model.Password!, user.PasswordHash,
                                                     user.PasswordSalt);
        if (!verified)
            throw new Exception("The password isn't correct");
        var mappedUser = Mapper.Map<TEntity, TDto>(user);
        //Generate Token
        var role = (mappedUser is AdminDto admin) ? admin.Role : null;
        var secretKey = _webConfiguration.GetSecretKey();
        var token = JwtHelper.GenerateToken(mappedUser, UserType, secretKey, role);
        mappedUser.Token = token;
        return mappedUser;
    }

    public virtual async Task<TDto> Register(TDtoRequest user)
    {
        if (user.Password == null)
            throw new Exception("password shouldn't be null");
        var mappedUser = Mapper.Map<TDtoRequest, TEntity>(user);
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
        var result = Mapper.Map<TEntity, TDto>(mappedUser);
        if (role != null)
        {
            var mappedRole = Mapper.Map<Role, RoleDto>(role);
            var token = JwtHelper.GenerateToken(result, UserType, secretKey, mappedRole);
            result.Token = token;
        }

        return result;
    }

    public override async Task<TDto> UpdateAsync(int id, TDtoRequest item)
    {
        var result = await Repository.SingleAsync(id);
        if (result == null)
            throw new Exception("item is not found");
        var oldPImage = result.Image;
        Mapper.Map(item, result);
        if (string.IsNullOrEmpty(item.Image) || item.Image == "none")
            result.Image = oldPImage;
        result.LastUpdate = DateTime.Now;
        if (item.Password != null)
        {
            HashingHelper.CreateHashPassword(item.Password, out var pHash, out var pSalt);
            result.PasswordHash = pHash;
            result.PasswordSalt = pSalt;
        }
        await Repository.Complete();
        var mappedResult = Mapper.Map<TEntity, TDto>(result);
        return mappedResult;
    }

    public Task PasswordRecoveryRequest(string email)
    {
        // var user = await Repository.SingleAsync(c => c.Email == email);
        // if (user == null)
        //     throw new Exception("this user isn't available");
        // EmailRequest recovery = new EmailRequest();
        // recovery.userId = user.Id;
        // recovery.ExpiredAt = DateTime.Now.AddHours(3);
        // var secretKey = _config.GetValue<string>("security:key");
        // var serializedObject = JsonSerializer.Serialize<EmailRequest>(recovery);
        // var encryptedText = AesEncryptionHelper.Encrypt(secretKey, serializedObject);
        // var path = Path.Combine(_webhostEnvironment.WebRootPath, "assets/templates/passwordRecovery.html");
        // var htmlContent = File.ReadAllText(path);
        // htmlContent = htmlContent.Replace("{{passwordRecoveryLink}}", $"https://www.apiplate.com/password_reset?key={encryptedText}");
        // await _smtpSerivce.SendMessageAsync(_config.GetValue<string>("smtp:username"), user.Email, "Password Recovery", htmlContent, MimeKit.Text.TextFormat.Html);
        throw new NotImplementedException();
    }

    public async Task<TDto> GetProfileAsync(int userId)
    {
        var target = await base.SingleAsync(userId);
        return target;
    }

    public Task PasswordRecovery(string key, string newPassword)
    {
        // var secretKey = _config.GetValue<string>("security:key");
        // var decryptedKey = AesEncryptionHelper.Decrypt(secretKey, key);
        // var emailRequestObject = JsonSerializer.Deserialize<EmailRequest>(decryptedKey);
        // if (DateTime.Now.CompareTo(emailRequestObject.ExpiredAt) == 1)
        //     throw new Exception("This key is expired");
        // var user = await Repository.SingleAsync(c => c.Id == emailRequestObject.userId);
        // if (user == null)
        //     throw new Exception("this user isn't available");
        // byte[] pHash, pSalt;
        // HashingHelper.CreateHashPassword(newPassword, out pHash, out pSalt);
        // user.PasswordHash = pHash;
        // user.PasswordSalt = pSalt;
        // await Repository.Complete();
        throw new NotImplementedException();
    }

    public async Task<TDto> UpdatePersonalInfo(int userId, JsonPatchDocument<TEntity> patchDoc)
    {
        if (patchDoc == null)
            throw new Exception("object shouldn't be null");
        var emailProp = patchDoc.Operations.SingleOrDefault(c => c.path == "/email");
        if (emailProp != null)
            throw new Exception("you can't change your email");
        var user = await Repository.SingleAsync(c => c.Id == userId);
        if (user == null)
            throw new Exception("this user isn't available");
        patchDoc.ApplyTo(user);
        await Repository.Complete();
        var result = Mapper.Map<TEntity, TDto>(user);
        return result;
    }

    public async Task ResetPassword(int id, string oldPassword, string newPassword)
    {
        var user = await Repository.SingleAsync(c => c.Id == id);
        if (user == null)
            throw new Exception("this user isn't available");
        var validOldPassword = user.PasswordSalt != null && user.PasswordHash != null &&
                               HashingHelper.VerifyPassword(oldPassword, user.PasswordHash, user.PasswordSalt);
        if (!validOldPassword)
            throw new Exception("invalid old password");
        HashingHelper.CreateHashPassword(newPassword, out var pHash, out var pSalt);
        user.PasswordHash = pHash;
        user.PasswordSalt = pSalt;
        await Repository.Complete();
    }

    public async Task<string> ChangePersonalPhoto(int id, IWebFormFile file)
    {
        var user = await Repository.SingleAsync(c => c.Id == id);
        if (user == null)
            throw new Exception("this user isn't available");
        var oldPhoto = user.Image;
        var result = await _filesManagerService.UploadSingleFile("assets/images/users", file);
        user.Image = result.Path.Replace("//", "/");
        await Repository.Complete();
        if (oldPhoto != null && _filesManagerService.FileExists(oldPhoto))
            _filesManagerService.DeleteFile(oldPhoto, "");
        return user.Image;
    }

    protected override string GetSearchPropValue(TEntity obj)
    {
        var targetType = typeof(TEntity);
        var searchProp = targetType.GetProperties().SingleOrDefault(c => string.Equals(c.Name, "username", StringComparison.OrdinalIgnoreCase));
        var propValue = searchProp?.GetValue(obj)?.ToString();
        return propValue ?? "";
    }
}

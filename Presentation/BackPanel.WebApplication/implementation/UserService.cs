using BackPanel.Application.DTOs;
using BackPanel.Application.DTOsRequests;
using BackPanel.Application.DTOs.Filters;
using BackPanel.Application.Extensions;
using BackPanel.Application.Helpers;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Enums;
using BackPanel.Persistence.Identity;
using BackPanel.SMTP.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MimeKit.Text;

namespace BackPanel.WebApplication.implementation;

public class UserService : IUserService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IIdentityRoleService _roleService;
    private readonly IWebConfiguration _configuration;
    private readonly ISmtpService _smtpService;

    public UserService(
        UserManager<AppUser> userManager,
        IIdentityRoleService roleService,
        IWebConfiguration configuration,
        ISmtpService smtpService)
    {
        _userManager = userManager;
        _roleService = roleService;
        _configuration = configuration;
        _smtpService = smtpService;
    }

    // ── Auth ─────────────────────────────────────────────────────────────────

    public async Task<AppUserDto> AuthenticateAsync(AuthenticationModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email)
                   ?? throw new Exception("Invalid credentials");

        if (!await _userManager.CheckPasswordAsync(user, model.Password))
            throw new Exception("Invalid credentials");

        if (user.Status == Status.Deleted)
            throw new Exception("Account not found");

        if (user.Status == Status.Disabled)
            throw new Exception("Account is suspended");

        var dto = await MapToDto(user);
        dto.Token = GenerateToken(dto);
        return dto;
    }

    public async Task<AppUserDto> RegisterAsync(AppUserDtoRequest request)
    {
        var user = new AppUser
        {
            UserName = request.UserName ?? request.Email,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Image = request.Image,
            IsManager = request.IsManager,
        };

        var result = await _userManager.CreateAsync(user, request.Password!);
        if (!result.Succeeded)
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

        if (request.RoleId.HasValue)
        {
            var role = await _roleService.GetByIdAsync(request.RoleId.Value);
            if (role?.Title != null)
                await _userManager.AddToRoleAsync(user, role.Title);
        }
        else
        {
            await _userManager.AddToRoleAsync(user, "ADMIN");
        }

        var dto = await MapToDto(user);
        dto.Token = GenerateToken(dto);
        return dto;
    }

    // ── Profile ───────────────────────────────────────────────────────────────

    public async Task<AppUserDto> GetProfileAsync(int userId)
    {
        var user = await FindUserOrThrowAsync(userId);
        return await MapToDto(user);
    }

    public async Task<AppUserDto> UpdateProfileAsync(int userId, AppUserDtoRequest request)
    {
        var user = await FindUserOrThrowAsync(userId);

        if (request.UserName != null) user.UserName = request.UserName;
        if (request.Email != null) user.Email = request.Email;
        if (request.PhoneNumber != null) user.PhoneNumber = request.PhoneNumber;
        if (request.Image != null) user.Image = request.Image;
        user.IsManager = request.IsManager;
        user.LastUpdate = DateTime.Now;

        if (request.RoleId.HasValue)
        {
            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            var newRole = await _roleService.GetByIdAsync(request.RoleId.Value);
            if (newRole?.Title != null)
                await _userManager.AddToRoleAsync(user, newRole.Title);
        }

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

        return await MapToDto(user);
    }

    public async Task<string> UpdatePhotoAsync(int userId, string imagePath)
    {
        var user = await FindUserOrThrowAsync(userId);
        user.Image = imagePath;
        user.LastUpdate = DateTime.Now;
        await _userManager.UpdateAsync(user);
        return imagePath;
    }

    // ── Password Recovery ─────────────────────────────────────────────────────

    public async Task<bool> RequestPasswordRecoveryAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return false;

        var code = new Random().Next(100000, 999999);
        await _userManager.SetAuthenticationTokenAsync(user, "PasswordRecovery", "Code", code.ToString());
        await _userManager.SetAuthenticationTokenAsync(user, "PasswordRecovery", "Expiry",
            DateTime.UtcNow.AddMinutes(30).ToString("o"));

        await _smtpService.SendMessageAsync(
            from: "noreply@backpanel.app",
            to: email,
            subject: "Password Recovery Code",
            content: $"<p>Your password recovery code is: <strong>{code}</strong></p><p>This code expires in 30 minutes.</p>",
            format: TextFormat.Html);

        return true;
    }

    public async Task<EmailRecoveryRequest> ValidatePasswordRecoveryCodeAsync(string email, int code)
    {
        var user = await _userManager.FindByEmailAsync(email)
                   ?? throw new Exception("User not found");

        var storedCode = await _userManager.GetAuthenticationTokenAsync(user, "PasswordRecovery", "Code");
        var storedExpiry = await _userManager.GetAuthenticationTokenAsync(user, "PasswordRecovery", "Expiry");

        if (storedCode != code.ToString())
            throw new Exception("Invalid recovery code");

        if (storedExpiry == null || DateTime.Parse(storedExpiry) < DateTime.UtcNow)
            throw new Exception("Recovery code has expired");

        return new EmailRecoveryRequest
        {
            UserId = user.Id,
            Code = code,
            ExpireAt = DateTime.Parse(storedExpiry)
        };
    }

    public async Task RecoverPasswordAsync(int userId, string newPassword)
    {
        var user = await FindUserOrThrowAsync(userId);
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
        if (!result.Succeeded)
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

        await _userManager.RemoveAuthenticationTokenAsync(user, "PasswordRecovery", "Code");
        await _userManager.RemoveAuthenticationTokenAsync(user, "PasswordRecovery", "Expiry");
    }

    public async Task<bool> ResetPasswordAsync(int userId, string oldPassword, string newPassword)
    {
        var user = await FindUserOrThrowAsync(userId);
        var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        if (!result.Succeeded)
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        return true;
    }

    // ── CRUD ──────────────────────────────────────────────────────────────────

    public async Task<IList<AppUserDto>> GetAllAsync()
    {
        var users = await _userManager.Users
            .Where(u => u.Status != Status.Deleted)
            .ToListAsync();

        var dtos = new List<AppUserDto>();
        foreach (var u in users)
            dtos.Add(await MapToDto(u));
        return dtos;
    }

    public async Task<Tuple<List<AppUserDto>, int>> GetAllFilteredAsync(ListFilter filter)
    {
        var all = (await GetAllAsync()).ToList();
        var total = all.Count;

        // Apply ordering
        all = all.OrderByProperty(filter.OrderBy, !filter.Descending).ToList();

        // Apply search expressions
        foreach (var expression in filter.SearchExpressions)
        {
            var lambda = ExpressionBuilder.BuildComparisonExpression<AppUserDto>(
                expression.PropName!, expression.Operator, expression.PropValue!);
            all = all.Where(lambda.Compile()).ToList();
        }

        // Apply unified search
        var expr = UnifiedSearchHelper.GetUnifiedSearchExpression<AppUserDto>(filter.Search);
        var func = expr?.Compile();
        all = all.Where(func ?? (_ => false)).ToList();

        // Apply pagination
        all = all
            .Skip((filter.PaginationFilter.PageIndex - 1) * filter.PaginationFilter.PageSize)
            .Take(filter.PaginationFilter.PageSize)
            .ToList();

        return Tuple.Create(all, total);
    }

    public async Task<byte[]> ExportToExcelAsync()
    {
        var all = await GetAllAsync();
        return DataExportHelper<AppUserDto>.ExportToExcel(all);
    }

    public async Task<AppUserDto> GetByIdAsync(int userId)
    {
        var user = await FindUserOrThrowAsync(userId);
        return await MapToDto(user);
    }

    public async Task DeleteAsync(int userId)
    {
        var user = await FindUserOrThrowAsync(userId);
        user.Status = Status.Deleted;
        user.LastUpdate = DateTime.Now;
        await _userManager.UpdateAsync(user);
    }

    public async Task ToggleActiveAsync(int userId)
    {
        var user = await FindUserOrThrowAsync(userId);
        user.Status = user.Status == Status.Active ? Status.Disabled : Status.Active;
        user.LastUpdate = DateTime.Now;
        await _userManager.UpdateAsync(user);
    }

    public async Task<IList<int>> GetAllUserIdsAsync()
    {
        return await _userManager.Users
            .Where(u => u.Status != Status.Deleted)
            .Select(u => u.Id)
            .ToListAsync();
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private async Task<AppUser> FindUserOrThrowAsync(int userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString())
                   ?? throw new Exception($"User {userId} not found");
        if (user.Status == Status.Deleted)
            throw new Exception($"User {userId} not found");
        return user;
    }

    private async Task<AppUserDto> MapToDto(AppUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        RoleDto? role = null;
        if (roles.Any())
            role = await _roleService.GetByNameAsync(roles[0]);

        return new AppUserDto
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Image = user.Image,
            IsManager = user.IsManager,
            Status = user.Status,
            CreatedAt = user.CreatedAt,
            LastUpdate = user.LastUpdate,
            RoleId = role?.Id,
            Role = role,
        };
    }

    private string GenerateToken(AppUserDto dto)
    {
        return JwtHelper.GenerateToken(dto, _configuration.GetSecretKey(), dto.Role);
    }
}

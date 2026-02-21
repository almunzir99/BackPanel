using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BackPanel.Application.DTOs;
using Microsoft.IdentityModel.Tokens;

namespace BackPanel.Application.Helpers;

public static class JwtHelper
{
    public static string GenerateToken(AppUserDto identity, string secretKey, RoleDto? role = default)
    {
        if (identity.Email == null)
            throw new Exception("user email shouldn't be null");

        const string identityType = "ADMIN";

        var claims = new List<Claim>
        {
            new Claim("id", identity.Id.ToString()),
            new Claim(ClaimTypes.Email, identity.Email),
            new Claim(ClaimTypes.Role, identityType),
            new Claim("user_type", identityType),
        };

        if (role == default)
        {
            claims.Add(new Claim("Manager", "true"));
        }
        else if (role.Title != null)
        {
            claims.Add(new Claim("admin_role", role.Title));
            claims.Add(new Claim("role_title", role.Title));
        }

        claims.Add(new Claim("type", identity.IsManager ? "manager" : "admin"));

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(secretKey);
        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature),
            Expires = DateTime.UtcNow.AddMonths(12),
        };
        var token = tokenHandler.CreateToken(descriptor);
        return tokenHandler.WriteToken(token);
    }
}

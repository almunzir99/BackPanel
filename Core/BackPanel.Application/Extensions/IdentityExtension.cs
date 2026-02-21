using System.Security.Claims;
using System.Security.Principal;

namespace BackPanel.Application.Extensions;

public static class IdentityExtension
{
    public static string GetClaimValue(this IPrincipal user, string claim)
    {
        var claimIdentity = user.Identity as ClaimsIdentity;
        var value = claimIdentity?.Claims
            .FirstOrDefault(c => c.Type.Equals(claim));
        if (value == null)
            return string.Empty;
        return value.Value;
    }
}
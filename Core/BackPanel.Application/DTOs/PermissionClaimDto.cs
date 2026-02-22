namespace BackPanel.Application.DTOs;

public class PermissionClaimDto
{
    public string ClaimType { get; set; } = "Permission";
    public string ClaimValue { get; set; } = string.Empty;
}

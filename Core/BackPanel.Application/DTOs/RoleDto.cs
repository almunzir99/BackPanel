namespace BackPanel.Application.DTOs;
public class RoleDto : DtoBase
{
    public string? Title { get; set; }
    public List<PermissionClaimDto> Permissions { get; set; } = new();
}
namespace BackPanel.Application.DTOs;
public class RoleDto : DtoBase
{
    public string? Title { get; set; }

    public PermissionDto? MessagesPermissions { get; set; }

    public PermissionDto? AdminsPermissions { get; set; }

    public PermissionDto? RolesPermissions { get; set; }

    public PermissionDto? CompanyInfosPermissions { get; set; }
}
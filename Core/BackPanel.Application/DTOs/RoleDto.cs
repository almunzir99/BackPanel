namespace BackPanel.Application.DTOs;

public class RoleDto
{
    public string? Title { get; set; }
    public PermissionDto? MessagesPermissions { get; set; }
    public PermissionDto? AdminsPermissions { get; set; }
    public PermissionDto? RolesPermissions { get; set; }
}
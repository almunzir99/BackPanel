using BackPanel.Application.DTOs;

namespace BackPanel.Application.DTOsRequests;

public class RoleDtoRequest
{
    public string? Title { get; set; }
    public PermissionDto? MessagesPermissions { get; set; }
    public PermissionDto? AdminsPermissions { get; set; }
    public PermissionDto? RolesPermissions { get; set; }
}
using BackPanel.Application.DTOs;

namespace BackPanel.Application.DTOsRequests;
public class RoleDtoRequest
{
    public string? Title { get; set; }
    public List<PermissionClaimDto> Permissions { get; set; } = new();
}
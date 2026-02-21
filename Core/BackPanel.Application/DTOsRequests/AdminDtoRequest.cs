namespace BackPanel.Application.DTOsRequests;

/// <summary>
/// Request DTO for admin users. Extends AppUserDtoRequest so admin-specific
/// fields can be added here without touching the base user request.
/// </summary>
public class AdminDtoRequest : AppUserDtoRequest
{
}
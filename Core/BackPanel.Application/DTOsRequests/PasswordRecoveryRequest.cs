namespace BackPanel.Application.DTOsRequests;

public class PasswordRecoveryRequest
{
        public string? Key { get; set; }
        public string? NewPassword { get; set; }
        public string? OldPassword { get; set; }
}
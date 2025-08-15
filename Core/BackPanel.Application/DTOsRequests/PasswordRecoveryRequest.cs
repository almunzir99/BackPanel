namespace BackPanel.Application.DTOsRequests;

public class PasswordRecoveryRequest
{
        public string? Key { get; set; }
        public required string NewPassword { get; set; }
        public required string OldPassword { get; set; }
}
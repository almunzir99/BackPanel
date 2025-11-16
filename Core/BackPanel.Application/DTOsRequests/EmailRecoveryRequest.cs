namespace BackPanel.Application.DTOsRequests;

public class EmailRecoveryRequest
{
    public int UserId { get; set; }
    public DateTime ExpireAt { get; set; }
    public int Code { get; set; }
}
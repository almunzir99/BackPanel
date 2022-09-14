namespace BackPanel.Domain.Entities;

public class Activity : EntityBase
{
    public int AdminId { get; set; }
    public Admin? Admin { get; set; }
    public string?  EffectedTable { get; set; }
    public int EffectedRowId { get; set; }
    public string? Action { get; set; }
    public Activity(){}
    public Activity(int adminId, string effectedTable, int effectedRowId, string action, System.DateTime createdAt)
    {
        AdminId = adminId;
        EffectedTable = effectedTable;
        EffectedRowId = effectedRowId;
        Action = action;
        CreatedAt = createdAt;
    }
}
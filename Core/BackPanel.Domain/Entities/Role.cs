namespace BackPanel.Domain.Entities;
public class Role : EntityBase
{
    public string? Title { get; set; }

    public Permission? MessagesPermissions { get; set; }

    public Permission? AdminsPermissions { get; set; }

    public Permission? RolesPermissions { get; set; }

    public Permission? CompanyInfosPermissions { get; set; }
}
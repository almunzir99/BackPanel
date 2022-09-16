using System.ComponentModel.DataAnnotations.Schema;

namespace BackPanel.Domain.Entities;

public class Admin : UserEntityBase
{
    public bool IsManager { get; set; }
    [ForeignKey("Role")]
    public int? RoleId { get; set; }
    public Role? Role { get; set; }
    public IList<Activity> Activities { get; set; } = new List<Activity>();
}
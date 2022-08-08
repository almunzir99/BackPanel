namespace BackPanel.Domain.Entities;

public class Permission : EntityBase
{
    public bool Create { get; set; }
    public bool Read { get; set; }
    public bool Update { get; set; }
    public bool Delete { get; set; }
}
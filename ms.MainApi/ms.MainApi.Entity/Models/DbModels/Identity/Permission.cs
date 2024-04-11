namespace ms.MainApi.Entity.Models.DbModels.Identity;

public class Permission : BaseEntity
{
    public int roleId { get; set; }
    public Role? role { get; set; } = new Role();

    public List<int>? actions { get; set; }

    public int permissionId { get; set; }
}
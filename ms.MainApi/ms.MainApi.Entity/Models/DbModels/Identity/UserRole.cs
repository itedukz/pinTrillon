namespace ms.MainApi.Entity.Models.DbModels.Identity;

public class UserRole : BaseEntity
{
    public int userId { get; set; }
    public User? user { get; set; } = new User();

    public int roleId { get; set; }
    public Role? role { get; set; } = new Role();
}
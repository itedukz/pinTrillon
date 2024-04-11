namespace ms.MainApi.Entity.Models.Dtos.Identities.UserRoles;

public class UserRoleCreateDto
{
    public int userId { get; set; }
    //public int roleId { get; set; } = 0;
    public List<int> rolesId { get; set; } = new List<int>();
}
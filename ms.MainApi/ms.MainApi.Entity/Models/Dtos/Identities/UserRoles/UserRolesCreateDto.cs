namespace ms.MainApi.Entity.Models.Dtos.Identities.UserRoles;

public class UserRolesCreateDto
{
    public int userId { get; set; } = 0;
    public List<int> roleIdList { get; set; } = new List<int>();
}
using ms.MainApi.Entity.Models.Dtos.Identities.Roles;
using ms.MainApi.Entity.Models.Dtos.Identities.Users;

namespace ms.MainApi.Entity.Models.Dtos.Identities.UserRoles;

public class UserRolesDto
{
    public UserDto? user { get; set; }

    public List<RoleShortDto>? roles { get; set; }
}
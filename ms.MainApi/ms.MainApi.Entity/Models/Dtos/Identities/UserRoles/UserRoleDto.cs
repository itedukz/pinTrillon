using ms.MainApi.Entity.Models.Dtos.Identities.Roles;
using ms.MainApi.Entity.Models.Dtos.Identities.Users;

namespace ms.MainApi.Entity.Models.Dtos.Identities.UserRoles;

public class UserRoleDto
{
    public UserDto? user { get; set; }

    public RoleDto? role { get; set; }
}
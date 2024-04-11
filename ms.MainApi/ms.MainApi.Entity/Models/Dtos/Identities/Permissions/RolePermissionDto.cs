using ms.MainApi.Entity.Models.Dtos.Identities.Roles;

namespace ms.MainApi.Entity.Models.Dtos.Identities.Permissions;

public class RolePermissionDto
{
    public RoleDto? role { get; set; } = new RoleDto();
    public List<PermissionActionDto>? permissions { get; set; }
}
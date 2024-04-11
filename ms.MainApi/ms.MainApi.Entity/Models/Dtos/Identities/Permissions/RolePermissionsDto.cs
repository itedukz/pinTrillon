using ms.MainApi.Entity.Models.Dtos.Identities.Roles;

namespace ms.MainApi.Entity.Models.Dtos.Identities.Permissions;

public class RolePermissionsDto
{
    public RoleDto? role { get; set; } = new RoleDto();
    public List<EnumItemDto>? permissions { get; set; }
}

using ms.MainApi.Entity.Models.Dtos.Identities.Roles;

namespace ms.MainApi.Entity.Models.Dtos.Identities.Permissions;

public class PermissionDto
{
    public RoleDto? role { get; set; } = new RoleDto();

    public List<EnumItemDto>? actions { get; set; }

    public EnumItemDto permission { get; set; }
}
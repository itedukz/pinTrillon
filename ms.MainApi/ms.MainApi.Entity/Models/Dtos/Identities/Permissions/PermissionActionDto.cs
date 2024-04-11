namespace ms.MainApi.Entity.Models.Dtos.Identities.Permissions;

public class PermissionActionDto
{
    public EnumItemDto? permission { get; set; }

    public List<PermissionActionDtlDto>? actions { get; set; }
}
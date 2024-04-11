namespace ms.MainApi.Entity.Models.Dtos.Identities.Permissions;

public class PermissionRemoveDto
{
    public int roleId { get; set; }
    public List<int>? actions { get; set; }
    public int permissionId { get; set; }
}

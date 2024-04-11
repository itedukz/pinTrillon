namespace ms.MainApi.Entity.Models.Dtos.Identities.PermissionServices;

public class PermissionList
{
    public bool canCreate { get; set; } = false;
    public bool canDelete { get; set; } = false;
    public bool canUpdate { get; set; } = false;
}
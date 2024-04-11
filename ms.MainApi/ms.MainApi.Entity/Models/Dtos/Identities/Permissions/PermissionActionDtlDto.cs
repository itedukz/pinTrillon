namespace ms.MainApi.Entity.Models.Dtos.Identities.Permissions;

public class PermissionActionDtlDto
{
    public int id { get; set; }
    public string name { get; set; } = $"";
    public int column { get; set; }
    public string group { get; set; } = $"";
    public bool isAllowed { get; set; } = false;
}

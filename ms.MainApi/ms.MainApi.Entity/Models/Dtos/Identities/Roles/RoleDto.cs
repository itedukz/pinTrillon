namespace ms.MainApi.Entity.Models.Dtos.Identities.Roles;

public class RoleDto
{
    public int id { get; set; }
    public string name { get; set; } = string.Empty;
    public string? description { get; set; }
}
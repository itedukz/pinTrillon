namespace ms.MainApi.Entity.Models.Dtos.Identities.Roles;

public class RoleCreateDto
{
    public string name { get; set; } = string.Empty;
    public string? description { get; set; }
}
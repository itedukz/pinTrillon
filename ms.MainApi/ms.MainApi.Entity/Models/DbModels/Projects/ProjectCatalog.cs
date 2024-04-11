namespace ms.MainApi.Entity.Models.DbModels.Projects;

public class ProjectCatalog : BaseEntity
{
    public string name { get; set; } = string.Empty;
    public string? description { get; set; }
}
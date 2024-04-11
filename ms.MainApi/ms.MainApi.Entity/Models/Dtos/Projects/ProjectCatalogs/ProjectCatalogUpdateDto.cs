namespace ms.MainApi.Entity.Models.Dtos.Projects.ProjectCatalogs;

public class ProjectCatalogUpdateDto
{
    public int id { get; set; }

    public string name { get; set; } = string.Empty;
    public string? description { get; set; }
}
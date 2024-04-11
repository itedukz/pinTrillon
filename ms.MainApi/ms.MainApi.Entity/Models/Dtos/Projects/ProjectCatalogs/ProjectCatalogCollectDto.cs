using ms.MainApi.Entity.Models.Dtos.Pictures;

namespace ms.MainApi.Entity.Models.Dtos.Projects.ProjectCatalogs;

public class ProjectCatalogCollectDto
{
    public int id { get; set; }
    public string name { get; set; } = string.Empty;
    public PictureDto? picture { get; set; }
    public int amount { get; set; } = 0;
}

using ms.MainApi.Entity.Models.Dtos.Products;
using ms.MainApi.Entity.Models.Dtos.Projects.ProjectCatalogs;

namespace ms.MainApi.Entity.Models.Pages.MainPages;

public class MainPage
{
    public List<ProjectCatalogCollectDto>? projectCatalogs { get; set; }
    public List<ProductShortDto>? products { get; set; }
}

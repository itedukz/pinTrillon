using ms.MainApi.Entity.Models.Dtos.Measures;
using ms.MainApi.Entity.Models.Dtos.Pictures;
using ms.MainApi.Entity.Models.Dtos.Projects.ProjectCatalogs;
using ms.MainApi.Entity.Models.Enums;

namespace ms.MainApi.Entity.Models.Dtos.Projects;

public class ProjectDto
{
    public int id { get; set; }
    public string name { get; set; } = string.Empty;
    public string article { get; set; } = string.Empty;
    public string? description { get; set; }
    public decimal price { get; set; }
    public decimal quadrature { get; set; }
    //public decimal height { get; set; }
    //public decimal width { get; set; }
    //public decimal length { get; set; }
    
    public MeasureDto? measure { get; set; }
    //public List<ColorInfo>? colors { get; set; }
    public ProjectCatalogDto? projectCatalog { get; set; }
    public List<PictureDto>? pictures { get; set; }
    public ProjectLayoutDto? layout { get; set; }
}
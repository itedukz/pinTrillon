using ms.MainApi.Entity.Models.Dtos.Measures;

namespace ms.MainApi.Entity.Models.Dtos.Projects;

public class ProjectCreateDto
{
    public string name { get; set; } = string.Empty;
    public string article { get; set; } = string.Empty;
    public string? description { get; set; }
    public decimal price { get; set; }
    public decimal quadrature { get; set; }
    //public decimal height { get; set; }
    //public decimal width { get; set; }
    //public decimal length { get; set; }

    public MeasureShortCreateDto? measure { get; set; }
    //public List<int>? colorsId { get; set; }
    public int projectCatalogId { get; set; }
}
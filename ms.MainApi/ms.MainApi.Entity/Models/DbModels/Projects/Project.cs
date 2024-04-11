using System.ComponentModel.DataAnnotations.Schema;

namespace ms.MainApi.Entity.Models.DbModels.Projects;

public class Project : BaseEntity
{
    public string name { get; set; } = string.Empty;
    public string article { get; set; } = string.Empty;
    public string? description { get; set; }
    public decimal price { get; set; }
    public decimal quadrature { get; set; }
    public decimal width { get; set; }
    public decimal length { get; set; }
    public decimal height { get; set; }
    //public List<int>? colorsId { get; set; }

    [Column(TypeName = "jsonb")]
    public string? properties { get; set; }

    public int projectCatalogId { get; set; }
    public ProjectCatalog? projectCatalog { get; set; }
}
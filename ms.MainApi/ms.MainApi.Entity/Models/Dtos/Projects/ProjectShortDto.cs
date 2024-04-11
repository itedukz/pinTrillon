using ms.MainApi.Entity.Models.Dtos.Measures;
using ms.MainApi.Entity.Models.Dtos.Pictures;

namespace ms.MainApi.Entity.Models.Dtos.Projects;

public class ProjectShortDto
{
    public int id { get; set; }
    public string name { get; set; } = string.Empty;
    //public string article { get; set; } = string.Empty;
    public string? description { get; set; }
    public decimal price { get; set; }
    public decimal quadrature { get; set; }
    public MeasureDto? measure { get; set; }

    public PictureDto? picture { get; set; }
    public PictureDto? layout { get; set; }
}

using ms.MainApi.Entity.Models.Dtos.Measures;

namespace ms.MainApi.Entity.Models.Dtos.Products;

public class ProductUpdateDto
{
    public int id { get; set; }
    public string name { get; set; } = string.Empty;
    public string? description { get; set; }
    public decimal price { get; set; }

    public int catalogId { get; set; }
    public int productArticleId { get; set; }
    public int brandId { get; set; }
    public List<int>? materialsId { get; set; }
    public List<int>? colorsId { get; set; }

    public MeasureCreateDto? measure { get; set; }
}
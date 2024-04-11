namespace ms.MainApi.Entity.Models.Dtos.Products.Brands;

public class BrandDto
{
    public int id { get; set; }
    public string name { get; set; } = string.Empty;
    public string? description { get; set; }
}
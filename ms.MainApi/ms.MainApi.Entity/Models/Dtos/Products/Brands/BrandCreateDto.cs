namespace ms.MainApi.Entity.Models.Dtos.Products.Brands;

public class BrandCreateDto
{
    public string name { get; set; } = string.Empty;
    public string? description { get; set; }
}
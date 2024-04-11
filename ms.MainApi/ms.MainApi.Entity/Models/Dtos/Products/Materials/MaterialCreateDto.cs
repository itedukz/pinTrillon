namespace ms.MainApi.Entity.Models.Dtos.Products.Materials;

public class MaterialCreateDto
{
    public string name { get; set; } = string.Empty;
    public string? description { get; set; }
}
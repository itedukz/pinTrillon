namespace ms.MainApi.Entity.Models.Dtos.Products.Materials;

public class MaterialDto
{
    public int id { get; set; }
    public string name { get; set; } = string.Empty;
    public string? description { get; set; }
}
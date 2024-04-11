namespace ms.MainApi.Entity.Models.Dtos.Catalogs;

public class CatalogCreateDto
{
    public string name { get; set; } = string.Empty;
    public string? description { get; set; }
}
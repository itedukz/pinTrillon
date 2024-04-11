namespace ms.MainApi.Entity.Models.Dtos.Catalogs;

public class CatalogDto
{
    public int id { get; set; }

    public string name { get; set; } = string.Empty;
    public string? description { get; set; }
}
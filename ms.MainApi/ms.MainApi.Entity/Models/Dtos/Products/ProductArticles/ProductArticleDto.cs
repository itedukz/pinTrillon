using ms.MainApi.Entity.Models.Dtos.Catalogs;

namespace ms.MainApi.Entity.Models.Dtos.Products.ProductArticles;

public class ProductArticleDto
{
    public int id { get; set; }

    public string name { get; set; } = string.Empty;
    public string? description { get; set; }
    public CatalogDto? catalog { get; set; }
}
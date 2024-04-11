namespace ms.MainApi.Entity.Models.Dtos.Products.ProductArticles;

public class ProductArticleCreateDto
{
    public string name { get; set; } = string.Empty;
    public string? description { get; set; }

    public int catalogId { get; set; }
}
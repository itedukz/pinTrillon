namespace ms.MainApi.Entity.Models.Dtos.Products.ProductArticles;

public class ProductArticleUpdateDto
{
    public int id { get; set; }

    public string name { get; set; } = string.Empty;
    public string? description { get; set; }

    public int catalogId { get; set; }
}
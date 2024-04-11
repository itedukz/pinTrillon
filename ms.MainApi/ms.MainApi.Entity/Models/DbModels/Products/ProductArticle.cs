using ms.MainApi.Entity.Models.DbModels.Catalogs;

namespace ms.MainApi.Entity.Models.DbModels.Products;

public class ProductArticle : BaseEntity
{
    public string name { get; set; } = string.Empty;
    public string? description { get; set; }

    public int catalogId { get; set; }
    public Catalog? catalog { get; set; }
}
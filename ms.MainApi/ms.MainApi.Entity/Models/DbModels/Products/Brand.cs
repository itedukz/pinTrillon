namespace ms.MainApi.Entity.Models.DbModels.Products;

public class Brand : BaseEntity
{
    public string name { get; set; } = string.Empty;
    public string? description { get; set; }
}
namespace ms.MainApi.Entity.Models.DbModels.Products;

public class ProductPicture : BaseEntity
{
    public int productId { get; set; }
    public Product? product { get; set; }

    public string FilePath { get; set; } = null!;
    public string FileName { get; set; } = null!;
    public bool isMain { get; set; } = false;
    public bool isProcessed { get; set; } = false;

    public byte[] picture { get; set; } = default!;
}
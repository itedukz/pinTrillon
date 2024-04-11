using ms.MainApi.Entity.Models.DbModels.Identity;
using ms.MainApi.Entity.Models.DbModels.Products;

namespace ms.MainApi.Entity.Models.DbModels.Baskets;

public class BasketProduct : BaseEntity
{
    public int userId { get; set; }
    public User? user { get; set; }

    public int productId { get; set; }
    public Product? product { get; set; }
}
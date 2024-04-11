using ms.MainApi.Entity.Models.DbModels.Products;

namespace ms.MainApi.Entity.Models.DbModels.Organizations;

public class OrganizationProduct
{
    public int organizationId { get; set; }
    public Organization? organization { get; set; }

    public int productId { get; set; }
    public Product? product { get; set; } = new Product();

}

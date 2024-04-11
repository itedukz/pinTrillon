using ms.MainApi.Entity.Models.DbModels.Identity;
using ms.MainApi.Entity.Models.DbModels.Products;
using ms.MainApi.Entity.Models.DbModels.Projects;

namespace ms.MainApi.Entity.Models.DbModels;

public class Basket : BaseEntity
{
    public int userId { get; set; }
    public User? user { get; set; }

    public int productId { get; set; }
    public Product? product { get; set; }

    public int projectId { get; set; }
    public Project? project { get; set; }

    public int referenceType { get; set; }
}
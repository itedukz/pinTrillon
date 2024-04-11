using ms.MainApi.Entity.Models.Dtos.Identities.Users;
using ms.MainApi.Entity.Models.Dtos.Products;
using ms.MainApi.Entity.Models.Dtos.Projects;

namespace ms.MainApi.Entity.Models.Dtos.Baskets;

internal class BasketDto
{
    public int id { get; set; }

    public UserDto? user { get; set; }
    public ProductDto? product { get; set; }
    public ProjectDto? project { get; set; }
    public EnumItemDto? referenceType { get; set; }
}
using ms.MainApi.Entity.Models.Dtos.Identities.Users;
using ms.MainApi.Entity.Models.Dtos.Products;
using ms.MainApi.Entity.Models.Dtos.Projects;

namespace ms.MainApi.Entity.Models.Dtos.Favourites;

internal class FavouriteDto
{
    public int id { get; set; }
    
    public UserDto? user { get; set; }
    public ProductShortDto? product { get; set; }
    public ProjectShortDto? project { get; set; }
    public EnumItemDto? referenceType { get; set; }
}
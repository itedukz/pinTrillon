using ms.MainApi.Entity.Models.DbModels.Identity;
using ms.MainApi.Entity.Models.DbModels.Projects;

namespace ms.MainApi.Entity.Models.DbModels.Favourites;

public class FavouriteProject : BaseEntity
{
    public int userId { get; set; }
    public User? user { get; set; }

    public int projectId { get; set; }
    public Project? project { get; set; }
}
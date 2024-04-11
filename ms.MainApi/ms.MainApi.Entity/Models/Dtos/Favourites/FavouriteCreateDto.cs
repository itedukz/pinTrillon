namespace ms.MainApi.Entity.Models.Dtos.Favourites;

internal class FavouriteCreateDto
{
    public int productId { get; set; }
    public int projectId { get; set; }
    public int referenceType { get; set; }
}
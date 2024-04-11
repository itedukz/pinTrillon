namespace ms.MainApi.Entity.Models.Dtos.Favourites;

internal class FavouriteUpdateDto
{
    public int id { get; set; }

    public int productId { get; set; }
    public int projectId { get; set; }
    public int referenceType { get; set; }
}

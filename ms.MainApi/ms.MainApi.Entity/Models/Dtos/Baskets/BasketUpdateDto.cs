namespace ms.MainApi.Entity.Models.Dtos.Baskets;

internal class BasketUpdateDto
{
    public int id { get; set; }

    public int productId { get; set; }
    public int projectId { get; set; }
    public int referenceType { get; set; }
}
namespace ms.MainApi.Entity.Models.Dtos.Baskets;

internal class BasketCreateDto
{
    public int productId { get; set; }
    public int projectId { get; set; }
    public int referenceType { get; set; }
}
using ms.MainApi.Entity.Models.Dtos.Pictures;

namespace ms.MainApi.Entity.Models.Dtos.Products.ProductPictures;

public class ProductPictureDto
{
    public ProductDto? product { get; set; }

    public List<PictureDto>? pictures { get; set; }
}
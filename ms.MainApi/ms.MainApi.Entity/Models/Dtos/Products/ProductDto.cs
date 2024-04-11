using ms.MainApi.Entity.Models.Dtos.Catalogs;
using ms.MainApi.Entity.Models.Dtos.Measures;
using ms.MainApi.Entity.Models.Dtos.Pictures;
using ms.MainApi.Entity.Models.Dtos.Products.Brands;
using ms.MainApi.Entity.Models.Dtos.Products.Materials;
using ms.MainApi.Entity.Models.Dtos.Products.ProductArticles;
using ms.MainApi.Entity.Models.Enums;

namespace ms.MainApi.Entity.Models.Dtos.Products;

public class ProductDto
{
    public int id { get; set; }
    public string name { get; set; } = string.Empty;
    public string? description { get; set; }
    public decimal price { get; set; }

    public CatalogDto? catalog { get; set; }
    public ProductArticleDto? productArticle { get; set; }
    public BrandDto? brand { get; set; }
    public List<MaterialDto>? materials { get; set; }
    public List<ColorInfo>? colors { get; set; }

    public MeasureDto? measure { get; set; }

    public List<PictureDto>? pictures { get; set; }
}
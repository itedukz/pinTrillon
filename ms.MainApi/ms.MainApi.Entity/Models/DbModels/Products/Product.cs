using ms.MainApi.Entity.Models.DbModels.Catalogs;
using System.ComponentModel.DataAnnotations.Schema;

namespace ms.MainApi.Entity.Models.DbModels.Products;

public class Product : BaseEntity
{
    public string name { get; set; } = string.Empty;
    public string? description { get; set; }
    public decimal price { get; set; }

    [Column(TypeName = "jsonb")] 
    public string? properties { get; set; }

    public int catalogId { get; set; }
    public Catalog? catalog { get; set; }
    
    public int productArticleId { get; set; }
    public ProductArticle? productArticle { get; set; }
    
    public int brandId { get; set; }
    public Brand? brand { get; set; }

    public List<int>? materialsId { get; set; }
    public List<int>? colorsId { get; set; }

    public decimal height { get; set; }  //высота
    public decimal width { get; set; }   //ширина
    public decimal length { get; set; }  //длина
    public int measureType { get; set; } //mm = 1, милиметр;    cm = 2, сантиметр;      m = 3, метр
}
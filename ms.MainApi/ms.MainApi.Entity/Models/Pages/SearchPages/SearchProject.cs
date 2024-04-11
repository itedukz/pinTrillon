using ms.MainApi.Entity.Models.Enums;

namespace ms.MainApi.Entity.Models.Pages.SearchPages;

public class SearchProject
{
    public SearchProjectParams? query { get; set; }
    public int sort { get; set; }
    public int page { get; set; } = 1;
    public int pageSize { get; set; } = 10;
}

public class SearchProjectParams
{
    public List<int>? catalogsId { get; set; }

    public decimal priceMin { get; set; }
    public decimal priceMax { get; set; }
    public decimal quadratureMin { get; set; }
    public decimal quadratureMax { get; set; }
    public decimal widthMin { get; set; }
    public decimal widthMax { get; set; }
    public decimal lengthMin { get; set; }
    public decimal lengthMax { get; set; }
    public decimal heightMin { get; set; }
    public decimal heightMax { get; set; }
}
namespace ms.MainApi.Entity.Models.Services;

public class QueryPagination
{
    public string? search { get; set; }
    public QueryParam? query { get; set; }
    public int page { get; set; } = 1;
    public int pageSize { get; set; } = 10;
}

public class QueryParam
{
    public string condition { get; set; } = "and";
    public List<QueryRules>? rules { get; set; }
}

public class QueryRules
{
    public string field { get; set; } = "";
    public string operators { get; set; } = "in";       //"equal"
    public string type { get; set; } = "string";
    public string? value { get; set; }
    public List<string>? values { get; set; }
}
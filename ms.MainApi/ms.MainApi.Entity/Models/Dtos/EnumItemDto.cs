namespace ms.MainApi.Entity.Models.Dtos;

public class EnumItemDto
{
    public int id { get; set; }
    public string name { get; set; } = $"";
}


public class entityPermissionDtlList : EnumItemDto
{
    public List<entityPermissionDtl> permissions { get; set; } = new List<entityPermissionDtl>();
}


public class entityPermissionDtl
{
    public int id { get; set; }
    public string name { get; set; } = $"";
    public int column { get; set; }
    public string group { get; set; } = $"";
}
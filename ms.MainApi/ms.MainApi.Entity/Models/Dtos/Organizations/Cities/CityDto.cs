namespace ms.MainApi.Entity.Models.Dtos.Organizations.Cities;

public class CityDto
{
    public int id { get; set; }
    public string name { get; set; } = string.Empty;
    public string? description { get; set; }
}
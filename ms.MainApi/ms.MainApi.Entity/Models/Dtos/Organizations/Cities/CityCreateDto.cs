namespace ms.MainApi.Entity.Models.Dtos.Organizations.Cities;

public class CityCreateDto
{
    public string name { get; set; } = string.Empty;
    public string? description { get; set; }
}
namespace ms.MainApi.Entity.Models.Dtos;

public class BaseDto
{
    public int id { get; set; }
    public string name { get; set; } = $"";
    public string? description { get; set; }
}
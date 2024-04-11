namespace ms.MainApi.Entity.Models.DbModels.Organizations;

public class City : BaseEntity
{
    public string name { get; set; } = string.Empty;
    public string? description { get; set; }
}
namespace ms.MainApi.Entity.Models.DbModels.Identity;

public class Role : BaseEntity
{
    public string name { get; set; } = string.Empty;
    public string description { get; set; } = string.Empty;
}
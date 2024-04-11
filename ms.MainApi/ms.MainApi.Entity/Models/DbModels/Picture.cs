namespace ms.MainApi.Entity.Models.DbModels;

public class Picture : BaseEntity
{
    public string FilePath { get; set; } = null!;
    public string FileName { get; set; } = null!;
}

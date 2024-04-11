namespace ms.MainApi.Entity.Models.DbModels.Identity;

public class User : BaseEntity
{
    public string firstName { get; set; } = string.Empty;

    public string? lastName { get; set; }
    public string? middleName { get; set; }
    public string? phoneNumber { get; set; }
    public string? fileName { get; set; }
    public string? filePath { get; set; }

    public string email { get; set; }
    public string passwordHash { get; set; } = string.Empty;

    public string? securityStamp { get; set; }
    public DateTime regDate { get; set; } = DateTime.Now;
    public bool isActive { get; set; }
}

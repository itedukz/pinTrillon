namespace ms.MainApi.Entity.Models.Dtos.Identities.Users;

public class UserCreateDto
{
    public string firstName { get; set; } = string.Empty;
    public string? lastName { get; set; }
    public string? middleName { get; set; }
    public string? phoneNumber { get; set; }

    public string email { get; set; } = string.Empty;
    public string password { get; set; } = string.Empty;
}
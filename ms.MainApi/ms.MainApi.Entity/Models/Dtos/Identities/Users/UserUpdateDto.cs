namespace ms.MainApi.Entity.Models.Dtos.Identities.Users;

public class UserUpdateDto
{
    public int id { get; set; }

    public string firstName { get; set; } = string.Empty;
    public string? lastName { get; set; }
    public string? middleName { get; set; }
    public string? phoneNumber { get; set; }

    //public string email { get; set; } = string.Empty;
}
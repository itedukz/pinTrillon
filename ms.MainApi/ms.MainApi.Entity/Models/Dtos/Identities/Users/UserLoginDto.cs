namespace ms.MainApi.Entity.Models.Dtos.Identities.Users;

public class UserLoginDto
{
    public string email { get; set; } = string.Empty;
    public string password { get; set; } = string.Empty;
}
namespace ms.MainApi.Entity.Models.Dtos.Identities.Users;

public class UserBanDto
{
    public int userId { get; set; }
    public bool isBan { get; set; } = true;
}
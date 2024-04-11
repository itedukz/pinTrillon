using ms.MainApi.Entity.Models.Dtos.Identities.Users;

namespace ms.MainApi.Entity.Models.Services;

public class Token
{
    public string Id { get; set; } = "";
    public string Value { get; set; } = "";
    public UserDto User { get; set; } = new UserDto();
    public DateTime ExpirationDate { get; set; }
}

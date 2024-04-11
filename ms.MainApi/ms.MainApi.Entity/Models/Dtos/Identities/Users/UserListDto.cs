using ms.MainApi.Core.GeneralHelpers;

namespace ms.MainApi.Entity.Models.Dtos.Identities.Users;

public class UserListDto
{
    public PaginationReturnModel pagination { get; set; } = new();
    public List<UserWithRolesDto> users { get; set; } = new();
}
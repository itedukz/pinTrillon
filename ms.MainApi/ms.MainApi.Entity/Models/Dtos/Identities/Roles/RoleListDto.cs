using ms.MainApi.Core.GeneralHelpers;

namespace ms.MainApi.Entity.Models.Dtos.Identities.Roles;

public class RoleListDto
{
    public PaginationReturnModel pagination { get; set; } = new();
    public List<RoleShortDto> roles { get; set; } = new();
}

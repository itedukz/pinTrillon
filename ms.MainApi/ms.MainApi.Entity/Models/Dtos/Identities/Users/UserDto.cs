using ms.MainApi.Entity.Models.Dtos.Identities.Roles;

namespace ms.MainApi.Entity.Models.Dtos.Identities.Users;

public class UserDto
{
    public int id { get; set; }

    public string firstName { get; set; } = string.Empty;
    public string? lastName { get; set; }
    public string? middleName { get; set; }
    public string? phoneNumber { get; set; }
    public string? fileName { get; set; }
    public string? filePath { get; set; }

    public string? email { get; set; }
    public bool isActive { get; set; } = true;
}

public class UserWithRolesDto
{
    public int id { get; set; }

    public string firstName { get; set; } = string.Empty;
    public string? lastName { get; set; }
    public string? middleName { get; set; }
    public string? phoneNumber { get; set; }
    public string? fileName { get; set; }
    public string? filePath { get; set; }

    public string email { get; set; } = string.Empty;
    public bool isActive { get; set; } = true;

    public List<RoleShortDto> roles { get; set; } = new List<RoleShortDto>();
}
using ms.MainApi.Entity.Models.Dtos.Identities.Users;
using ms.MainApi.Entity.Models.Dtos.Organizations.Cities;
using ms.MainApi.Entity.Models.Dtos.Pictures;

namespace ms.MainApi.Entity.Models.Dtos.Organizations;

public class OrganizationDto
{
    public int id { get; set; }
    public string name { get; set; } = string.Empty;
    public string? description { get; set; }

    public List<string>? phoneNumber { get; set; }
    public string? logoFileName { get; set; }
    public string? logoFilePath { get; set; }
    public PictureDto? banner { get; set; }

    public UserDto? user { get; set; }
    public CityDto? city { get; set; }

    public DateTime regDate { get; set; } = DateTime.Now;
    public bool isActive { get; set; } = true;
}

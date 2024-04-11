namespace ms.MainApi.Entity.Models.Dtos.Organizations;

public class OrganizationCreateDto
{
    public string name { get; set; } = string.Empty;
    public string? description { get; set; }

    public List<string>? phoneNumber { get; set; }
    //public string? logoFileName { get; set; }
    //public string? logoFilePath { get; set; }

    //public int userId { get; set; }
    public int cityId { get; set; }

    public DateTime regDate { get; set; } = DateTime.Now;
    public bool isActive { get; set; } = true;
}
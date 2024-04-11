namespace ms.MainApi.Entity.Models.DbModels.Organizations;

public class OrganizationPicture : BaseEntity
{
    public int organizationId { get; set; }
    public Organization? organization { get; set; }

    public string FilePath { get; set; } = null!;
    public string FileName { get; set; } = null!;
}
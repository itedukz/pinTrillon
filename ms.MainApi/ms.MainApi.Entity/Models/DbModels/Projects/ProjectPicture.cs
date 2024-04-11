namespace ms.MainApi.Entity.Models.DbModels.Projects;

public class ProjectPicture : BaseEntity
{
    public int projectId { get; set; }
    public Project? project { get; set; }

    public string FilePath { get; set; } = null!;
    public string FileName { get; set; } = null!;
    public bool isMain { get; set; } = false;

    public byte[] picture { get; set; } = default!;
}
namespace ms.MainApi.Entity.Models.DbModels.Projects;

public class ProjectLayout : BaseEntity
{
    public int projectId { get; set; }
    public Project? project { get; set; }

    public string FilePath { get; set; } = null!;
    public string FileName { get; set; } = null!;
}
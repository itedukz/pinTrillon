namespace ms.MainApi.Entity.Models.Dtos.Projects;

public class ProjectLayoutDto
{
    public int id { get; set; }
    public int projectId { get; set; }
    public string FilePath { get; set; } = null!;
    public string FileName { get; set; } = null!;
}

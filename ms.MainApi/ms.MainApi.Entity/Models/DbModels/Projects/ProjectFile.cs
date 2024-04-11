namespace ms.MainApi.Entity.Models.DbModels.Projects;

public class ProjectFile : BaseEntity
{
    public string name { get; set; } = string.Empty;
    public string? description { get; set; }
    public bool isFree { get; set; }        //выдается безплатно или после покупки

    public string FilePath { get; set; } = null!;
    public string FileName { get; set; } = null!;

    public int projectId { get; set; }
    public Project? project { get; set; }
}
namespace ms.MainApi.Entity.Models.Dtos.Pictures;

public class PictureDto
{
    public int id { get; set; }

    public string FilePath { get; set; } = null!;
    public string FileName { get; set; } = null!;
    public bool isMain { get; set; }
}

public class PictureShortDto
{
    public string FilePath { get; set; } = null!;
    public string FileName { get; set; } = null!;
}
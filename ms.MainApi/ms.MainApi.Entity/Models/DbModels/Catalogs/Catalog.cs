using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace ms.MainApi.Entity.Models.DbModels.Catalogs;

public class Catalog : BaseEntity
{
    public string name { get; set; } = string.Empty;
    public string? description { get; set; }

    [Column(TypeName = "jsonb")]
    public string? properties { get; set; }
}
using ms.MainApi.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ms.MainApi.Entity.Models;

public class BaseEntity : IEntity
{
    [Key] public int id { get; set; }

    public int createdBy { get; set; }
    public DateTime createdAt { get; set; }

    public int? updatedBy { get; set; } = 0;
    public DateTime? updatedAt { get; set; }

    public bool isDeleted { get; set; } = false;
    public int? deletedBy { get; set; } = 0;
    public DateTime? deletedAt { get; set; }

}

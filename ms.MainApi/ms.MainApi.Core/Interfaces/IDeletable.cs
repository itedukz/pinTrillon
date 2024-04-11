namespace ms.MainApi.Core.Interfaces;

public interface IDeletable
{
    bool isDeleted { get; set; }
    int? deletedBy { get; set; }
    DateTime? deletedAt { get; set; }
}

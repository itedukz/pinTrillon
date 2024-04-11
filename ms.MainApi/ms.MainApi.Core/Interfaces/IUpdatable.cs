namespace ms.MainApi.Core.Interfaces;

public interface IUpdatable
{
    int? updatedBy { get; set; }
    DateTime? updatedAt { get; set; }
}

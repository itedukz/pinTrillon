namespace ms.MainApi.Core.Interfaces;

public interface ICreatable
{
    int createdBy { get; set; }
    DateTime createdAt { get; set; }
}

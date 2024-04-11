using System.ComponentModel.DataAnnotations;

namespace ms.MainApi.Core.Interfaces;

public interface IEntity : ICreatable, IUpdatable, IDeletable
{
    [Key] public int id { get; set; }
}

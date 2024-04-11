namespace ms.MainApi.Core.GeneralHelpers.RabbitMq.Models;

public class RabbitMqConnectionFactoryModel
{
    public string HostName { get; set; } = null!;
    public int? Port { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
}
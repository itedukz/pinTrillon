namespace ms.MainApi.Core.GeneralHelpers.RabbitMq.Models;

public class RabbitMqQueueInformationModel
{
    public string QueueName { get; set; } = null!;
    public bool Durable { get; set; } = false;
    public bool Exclusive { get; set; } = false;
    public bool AutoDelete { get; set; } = false;
}
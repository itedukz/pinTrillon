using ms.MainApi.Entity.Models.Enums;

namespace ms.MainApi.Entity.Models.RabbitMq;

public class MessageNotificationDto
{
    public int referenceId { get; set; }
    public MessageTypes MessageType { get; set; }
}

namespace ms.MainApi.Core.GeneralHelpers.RabbitMq.Models;

public class RabbitMqSendRequestModel
{
    public RabbitMqConnectionFactoryModel Factory { get; set; } = new()
    {
        HostName = "rmq.agrar.kz",      //http://rmq.agrar.kz/
        //Port = 80,
        //UserName = "guest",
        //Password = "guest",
    };

    public RabbitMqQueueInformationModel QueueInformation { get; set; } = null!;
    public string Message { get; set; } = "";
}
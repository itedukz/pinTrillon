using ms.MainApi.Core.GeneralHelpers.RabbitMq.Models;
using RabbitMQ.Client;
using System.Text;

namespace ms.MainApi.Core.GeneralHelpers.RabbitMq;

public interface IRabbitMqHelperRepository
{
    void CreateMessage(RabbitMqSendRequestModel form);
}

public class RabbitMqHelperRepository : IRabbitMqHelperRepository
{
    public void CreateMessage(RabbitMqSendRequestModel form)
    {
        try
        {
            var factory = new ConnectionFactory() { 
                HostName = form.Factory.HostName                
            };

            if (form.Factory.Port != null)
                factory.Port = form.Factory.Port.Value;

            if (!string.IsNullOrEmpty(form.Factory.UserName))
                factory.UserName = form.Factory.UserName;
            
            if (!string.IsNullOrEmpty(form.Factory.Password))
                factory.Password = form.Factory.Password;

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(
                queue: form.QueueInformation.QueueName,
                durable: form.QueueInformation.Durable,
                exclusive: form.QueueInformation.Exclusive,
                autoDelete: form.QueueInformation.AutoDelete,
                arguments: null
            );

            var body = Encoding.UTF8.GetBytes(form.Message);

            channel.BasicPublish(
                exchange: "",
                routingKey: form.QueueInformation.QueueName,
                basicProperties: null,
                body: body
            );
        }
        catch { }
    }
}
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ms.MainApi.DataAccess.Products;
using ms.MainApi.Entity.Models.DbModels.Products;
using ms.MainApi.Entity.Models.RabbitMq;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Net.Security;
using System.Text;

namespace ms.MainApi.Business.Services.RabbitMqConsumers;

public class ProductPictureSendService : BackgroundService
{
    #region DI
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly string _queueName = "product-picture-sender-bg-service";

    public ProductPictureSendService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }
    #endregion

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory()
        {
            HostName = "82.200.245.234",                  //http://rmq.agrar.kz/  "rmq.agrar.kz"  82.200.245.234
            Port = 5672,
            UserName = "guest",
            Password = "guest",
            AutomaticRecoveryEnabled = true,
            Ssl = new SslOption()
            {
                Enabled = true,
                AcceptablePolicyErrors = SslPolicyErrors.RemoteCertificateNameMismatch | SslPolicyErrors.RemoteCertificateChainErrors
            }
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.QueueDeclare(
            queue: _queueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );
        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += OnConsumerOnReceived;
        channel.BasicConsume(
            queue: _queueName,
            autoAck: true,
            consumer: consumer
        );
        var tcs = new TaskCompletionSource<bool>();
        stoppingToken.Register(s => ((TaskCompletionSource<bool>)s).SetResult(true), tcs);
        await tcs.Task;
    }

    private async void OnConsumerOnReceived(object model, BasicDeliverEventArgs ea)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var _entityDal = _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IProductPictureDal>();
        var body = ea.Body.ToArray();
        var jsonStr = Encoding.UTF8.GetString(body);
        var picture = JsonConvert.DeserializeObject<MessageNotificationDto>(jsonStr);
        if (picture == null) return;

        ProductPicture? entity = await _entityDal.GetAsync(i => i.id == picture!.referenceId);
        if(entity != null)
        {
            entity.isProcessed = true;
            await _entityDal.UpdateAsync(entity);
        }
    }
}

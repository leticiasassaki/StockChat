using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using StockChat.Domain.Interfaces.Infra.Repository;
using StockChat.Domain.Models;
using System.Text;
using System.Text.Json;

namespace StockChat.MessageConsumer
{
    public class InsertMessageConsumer : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;

        public InsertMessageConsumer(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var connectionString = _configuration["Rabbit:SaveChatMessageQueue:ConnectionString"];
            var queueName = _configuration["Rabbit:SaveChatMessageQueue:QueueName"];
            var durable = Convert.ToBoolean(_configuration["Rabbit:SaveChatMessageQueue:Durable"]);

            var factory = new ConnectionFactory()
            {
                Uri = new Uri(connectionString),
                DispatchConsumersAsync = true
            };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(queue: queueName,
                                durable: true,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += async (object sender, BasicDeliverEventArgs e) =>
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());
                var post = JsonSerializer.Deserialize<Message>(message);


                if (post == null)
                    return;

                using (var scope = _serviceProvider.CreateScope())
                {
                    var messageRepository = scope.ServiceProvider.GetRequiredService<IMessageRepository>();
                    await messageRepository.Add(post);

                    channel.BasicAck(e.DeliveryTag, false);
                };
            };

            channel.BasicConsume(queue: queueName,
                autoAck: false,
                consumer: consumer);
        }
    }
}

﻿using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using StockChat.Domain.Interfaces.Infra.Queue;
using System.Text;
using System.Text.Json;

namespace StockChat.Infrastructure.Queue
{
    public class Queue : IQueue
    {
        private readonly IConfiguration _configuration;

        public Queue(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Enqueue<T>(string queue, T message)
        {
            var connectionString = _configuration[$"Rabbit:{queue}:ConnectionString"];
            var queueName = _configuration[$"Rabbit:{queue}:QueueName"];
            var durable = Convert.ToBoolean(_configuration[$"Rabbit:{queue}:Durable"]);

            var factory = new ConnectionFactory()
            {
                Uri = new Uri(connectionString)
            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: queueName,
                                 durable: durable,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            channel.BasicPublish(exchange: "",
                                 routingKey: queueName,
                                 basicProperties: null,
                                 body: Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message)));
        }
    }
}

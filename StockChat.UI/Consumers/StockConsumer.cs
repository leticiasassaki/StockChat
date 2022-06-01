﻿using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using StockChat.Domain.Dto.External;
using StockChat.UI.Hubs;
using System.Text;

namespace StockChat.UI.Consumers
{
    public class StockConsumer : BackgroundService
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IConfiguration _configuration;

        public StockConsumer(IHubContext<ChatHub> hubContext,
                             IConfiguration configuration)
        {
            _hubContext = hubContext;
            _configuration = configuration;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var connectionString = _configuration["Rabbit:StockQueue:ConnectionString"];
            var queueName = _configuration["Rabbit:StockQueue:QueueName"];
            var durable = Convert.ToBoolean(_configuration["Rabbit:StockQueue:Durable"]);

            var factory = new ConnectionFactory()
            {
                Uri = new Uri(connectionString),
                DispatchConsumersAsync = true
            };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(queue: queueName,
                                durable: durable,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += HandleMessage;
            channel.BasicConsume(queue: queueName,
                autoAck: true,
                consumer: consumer);
        }

        public async Task HandleMessage(object sender, BasicDeliverEventArgs e)
        {
            var message = Encoding.UTF8.GetString(e.Body.ToArray());
            var stockData = JsonConvert.DeserializeObject<StockMessageDto>(message);

            if (stockData == null)
                return;

            if (stockData.StockValue == "N/D")
                await _hubContext.Clients.Client(stockData.Caller).SendAsync("ReceiveMessage", "stock-bot", "Stock code not found");
            else
                await _hubContext.Clients.Client(stockData.Caller).SendAsync("ReceiveMessage", "stock-bot", $"{stockData.StockCode} quote is ${stockData.StockValue} per share");
        }
    }
}

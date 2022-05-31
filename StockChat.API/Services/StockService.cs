using RabbitMQ.Client;
using StockChat.API.Mappers;
using StockChat.API.Models.Csv;
using StockChat.Domain.Dto.External;
using System.Text;
using System.Text.Json;
using TinyCsvParser;

namespace StockChat.API.Services
{
    public class StockService : IStockService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public StockService(IHttpClientFactory httpClientFactory,
                            IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<StockCsv> GetStock(string stockCode, string caller)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var stockBaseUrl = _configuration["Urls:Stock"];
            var stockUrl = string.Format(stockBaseUrl, stockCode);
            using var stream = await httpClient.GetStreamAsync(stockUrl);
            StreamReader reader = new(stream);
            string text = reader.ReadToEnd();

            var csvParserOptions = new CsvParserOptions(true, ',');
            var csvReaderOptions = new CsvReaderOptions(new[] { Environment.NewLine });
            var csvMapper = new StockMapping();
            var csvParser = new CsvParser<StockCsv>(csvParserOptions, csvMapper);

            var result = csvParser.ReadFromString(csvReaderOptions, text).FirstOrDefault()!.Result;
            var message = new StockMessageDto
            {
                StockCode = stockCode,
                Caller = caller,
                StockValue = result.Close,
            };

            SendToRabbit(message);
            return result;
        }

        private void SendToRabbit(StockMessageDto stockMessage)
        {
            var connectionString = _configuration["Rabbit:StockQueue:ConnectionString"];
            var queueName = _configuration["Rabbit:StockQueue:QueueName"];
            var durable = Convert.ToBoolean(_configuration["Rabbit:StockQueue:Durable"]);

            var message = new { stockMessage.StockCode, stockMessage.StockValue, stockMessage.Caller };
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

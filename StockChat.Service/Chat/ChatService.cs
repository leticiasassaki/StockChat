using StockChat.Domain.Interfaces.Infra.Queue;
using StockChat.Domain.Interfaces.Service;
using StockChat.Domain.Models;

namespace StockChat.Service.Chat
{
    public class ChatService : IChatService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IQueue _queue;
        public ChatService(IHttpClientFactory httpClientFactory,
                           IQueue queue)
        {
            _httpClientFactory = httpClientFactory;
            _queue = queue;
        }

        public (bool success, string errors) EnqueueChatMessageToBeSaved(Message message)
        {
            if (message == null)
                return (false, "Message cannot be null");

            if (string.IsNullOrWhiteSpace(message.Owner))
                return (false, "Owner is required");

            if (string.IsNullOrWhiteSpace(message.Content))
                return (false, "Content cannot be empty");

            if (message.Content.Length > 1000)
                return (false, "Message maximum lenght is 1000 chars");

            _queue.Enqueue<Message>("SaveChatMessageQueue", message);

            return (true, String.Empty);
        }

        public async Task<bool> GetQuotation(string stockCode, string caller)
        {
            var client = _httpClientFactory.CreateClient("StockAPI");
            var response = await client.GetAsync($"/GetStock/{stockCode}/{caller}");
            return response.IsSuccessStatusCode;
        }
    }
}

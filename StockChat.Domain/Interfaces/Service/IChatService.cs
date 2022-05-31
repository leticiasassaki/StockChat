using StockChat.Domain.Models;

namespace StockChat.Domain.Interfaces.Service
{
    public interface IChatService
    {
        (bool success, string errors) EnqueueChatMessageToBeSaved(Message message);
        Task<bool> GetQuotation(string stockCode, string caller);
    }
}

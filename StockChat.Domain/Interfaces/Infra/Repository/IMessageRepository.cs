using StockChat.Domain.Models;

namespace StockChat.Domain.Interfaces.Infra.Repository
{
    public interface IMessageRepository
    {
        Task Add(Message message);
    }
}

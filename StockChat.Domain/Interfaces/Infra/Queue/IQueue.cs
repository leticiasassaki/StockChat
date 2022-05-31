namespace StockChat.Domain.Interfaces.Infra.Queue
{
    public interface IQueue
    {
        void Enqueue<T>(string queue, T message);
    }
}

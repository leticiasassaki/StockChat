namespace StockChat.Domain.Models
{
    public class BaseEntity
    {
        public Guid Id { get; } = Guid.NewGuid();
        public DateTime CreationDate { get; } = DateTime.Now;
    }
}

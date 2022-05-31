namespace StockChat.Domain.Models
{
    public class Message : BaseEntity
    {
        public Message(string owner, string content)
        {
            Owner = owner;
            Content = content;
        }

        public string Owner { get; set; }
        public string Content { get; set; }
    }
}

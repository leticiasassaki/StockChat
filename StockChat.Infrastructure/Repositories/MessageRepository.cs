using StockChat.Domain.Interfaces.Infra.Repository;
using StockChat.Domain.Models;
using StockChat.Infrastructure.Data;

namespace StockChat.Infrastructure.Repositories
{
    public class MessageRepository : BaseRepository<Message>, IMessageRepository
    {
        public MessageRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task Add(Message post) => await DbSet.AddAsync(post);
    }
}

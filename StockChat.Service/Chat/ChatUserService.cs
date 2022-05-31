using StockChat.Domain.Dto.Chat;
using StockChat.Domain.Interfaces.Service;

namespace StockChat.Service.Chat
{
    public class ChatUserService : IChatUserService
    {
        public List<ActiveUserDto> ChatUsers { get; } = new List<ActiveUserDto>();

        public void AddUser(ActiveUserDto user)
        {
            if (GetByEmail(user.Email) == null)
                ChatUsers.Add(user);
        }

        public void RemoveUser(ActiveUserDto user) =>
            ChatUsers.Remove(user);

        public ActiveUserDto GetByConnection(string connectionId) =>
            ChatUsers.FirstOrDefault(x => x.ConnectionId == connectionId);

        public ActiveUserDto GetByEmail(string email) =>
            ChatUsers.FirstOrDefault(x => x.Email == email);

        public int GetCount() => ChatUsers.Count;

        public void RemoveUser(string connectionid)
        {
            var user = GetByConnection(connectionid);
            if (user == null)
                return;

            RemoveUser(user);
        }
    }
}

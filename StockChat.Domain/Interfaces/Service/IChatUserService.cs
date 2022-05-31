using StockChat.Domain.Dto.Chat;

namespace StockChat.Domain.Interfaces.Service
{
    public interface IChatUserService
    {
        List<ActiveUserDto> ChatUsers { get; }
        void AddUser(ActiveUserDto user);
        ActiveUserDto GetByConnection(string connectionId);
        ActiveUserDto GetByEmail(string email);
        void RemoveUser(ActiveUserDto user);
        void RemoveUser(string connectionid);
    }
}

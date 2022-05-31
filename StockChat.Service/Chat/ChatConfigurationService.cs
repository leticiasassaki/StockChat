using Microsoft.Extensions.Configuration;
using StockChat.Domain.Interfaces.Service;

namespace StockChat.Service.Chat
{
    public class ChatConfigurationService : IChatConfigurationService
    {
        private readonly IConfiguration _configuration;

        public ChatConfigurationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public int GetMessageLimit()
        {
            var limit = _configuration["ChatConfiguration:MaxAllowedMessage"];
            return Convert.ToInt32(limit);
        }
    }
}

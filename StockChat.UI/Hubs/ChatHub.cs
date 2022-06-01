using Microsoft.AspNetCore.SignalR;
using StockChat.Domain.Dto.Chat;
using StockChat.Domain.Interfaces.Service;
using StockChat.Domain.Models;

namespace StockChat.UI.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatUserService _chatUserService;
        private readonly IChatConfigurationService _configuration;
        private readonly IChatService _chatService;
        public ChatHub(IChatUserService chatUserService,
                    IChatConfigurationService configuration,
                    IChatService chatService)
        {
            _chatUserService = chatUserService;
            _configuration = configuration;
            _chatService = chatService;
        }

        public override async Task OnConnectedAsync()
        {
            var email = Context.User.Identity.Name;
            var connectionId = Context.ConnectionId;
            var chatActiveUserDto = new ActiveUserDto(connectionId, email);
            _chatUserService.AddUser(chatActiveUserDto);
            await SetUsers();
            await SetMessageLimit();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            _chatUserService.RemoveUser(Context.ConnectionId);
        }

        public async Task SendMessage(string message)
        {

            if (message.StartsWith("/stock="))
            {
                var stockCode = message.Replace("/stock=", "");
                await _chatService.GetQuotation(stockCode, Context.ConnectionId);
                return;
            }

            var user = Context.User.Identity.Name;
            if (!SaveMessage(message, user))
                return;

            await Clients.All.SendCoreAsync("ReceiveMessage", GetArray(user, message));
        }

        private async Task SetUsers() =>
            await Clients.All.SendCoreAsync("SetUsers", GetArray(_chatUserService.ChatUsers.Select(x => x.Email).ToArray()));

        private async Task SetMessageLimit()
        {
            int messageLimit = _configuration.GetMessageLimit();
            await Clients.All.SendCoreAsync("SetMessageLimit", GetArray(messageLimit));

        }

        private object[] GetArray(string[] obj)
        {
            return new object[] { obj };
        }
        private object[] GetArray(params object[] args)
        {
            return args.ToArray();
        }

        private bool SaveMessage(string content, string user)
        {
            var message = new Message(user, content);
            var (success, _) = _chatService.EnqueueChatMessageToBeSaved(message);

            return success;
        }
    }
}

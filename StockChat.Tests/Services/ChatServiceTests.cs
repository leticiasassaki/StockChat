using StockChat.Domain.Models;
using StockChat.Service.Chat;

namespace StockChat.Tests.Services
{
    public class ChatServiceTests
    {
        private readonly AutoMocker _mocker;
        private readonly ChatService _chatService;

        public ChatServiceTests()
        {
            _mocker = new AutoMocker();

            _chatService = _mocker.CreateInstance<ChatService>();
        }

        [Fact(DisplayName = "Should return error message")]
        public void EnqueueChatMessageToBeSaved_InvalidMessage_ShouldReturnError()
        {
            // Act
            var result = _chatService.EnqueueChatMessageToBeSaved(null);

            // Assert
            result.success.Should().BeFalse();
            result.errors.Should().Be("Message cannot be null");
        }

        [Fact(DisplayName = "Should return error message")]
        public void EnqueueChatMessageToBeSaved_InvalidOwner_ShouldReturnError()
        {
            // Arrange
            var message = new Message("", "Content");

            // Act
            var result = _chatService.EnqueueChatMessageToBeSaved(message);

            // Assert
            result.success.Should().BeFalse();
            result.errors.Should().Be("Owner is required");
        }

        [Fact(DisplayName = "Should return error message")]
        public void EnqueueChatMessageToBeSaved_InvalidContent_ShouldReturnError()
        {
            // Arrange
            var message = new Message("Owner", "");

            // Act
            var result = _chatService.EnqueueChatMessageToBeSaved(message);

            // Assert
            result.success.Should().BeFalse();
            result.errors.Should().Be("Content cannot be empty");
        }

        [Fact(DisplayName = "Should return success")]
        public void EnqueueChatMessageToBeSaved_ValidMessage_ShouldReturnSuccess()
        {
            // Arrange
            var message = new Message("Teste", "Teste");

            // Act
            var result = _chatService.EnqueueChatMessageToBeSaved(message);

            // Assert
            result.success.Should().BeTrue();
        }
    }
}

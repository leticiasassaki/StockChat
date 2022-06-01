using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using StockChat.Service.Chat;

namespace StockChat.Tests.Services
{
    public class ChatConfigurationServiceTests
    {
        private readonly AutoMocker _mocker;
        private ChatConfigurationService _chatConfigurationService;

        public ChatConfigurationServiceTests()
        {
            _mocker = new AutoMocker();
        }

        [Theory(DisplayName = "Should return MaxAllowedMessage")]
        [InlineData("50")]
        [InlineData("100")]
        [InlineData("0")]
        public void GetMessageLimit_ValidConfig_ShouldReturnMaxAllowedMessage(string maxAllowedMessage)
        {
            // Arrange
            var appSettings = new Dictionary<string, string>
            {
                {"ChatConfiguration:MaxAllowedMessage", maxAllowedMessage},
            };

            var configuration = new ConfigurationBuilder()
                                    .AddInMemoryCollection(appSettings)
                                    .Build();

            _mocker.Use<IConfiguration>(configuration);
            _chatConfigurationService = _mocker.CreateInstance<ChatConfigurationService>();

            // Act
            var result = _chatConfigurationService.GetMessageLimit();

            // Assert
            result.ToString().Should().Be(maxAllowedMessage);
        }

        [Fact(DisplayName = "Should return zero to message limit")]
        public void GetMessageLimit_WithoutConfig_ShouldReturnZero()
        {
            // Arrange
            _chatConfigurationService = _mocker.CreateInstance<ChatConfigurationService>();

            // Act
            var result = _chatConfigurationService.GetMessageLimit();

            // Assert
            result.Should().Be(0);
        }
    }
}

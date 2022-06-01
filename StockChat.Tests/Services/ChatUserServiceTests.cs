using StockChat.Domain.Dto.Chat;
using StockChat.Service.Chat;

namespace StockChat.Tests.Services
{
    public class ChatUserServiceTests
    {
        private readonly AutoMocker _mocker;
        private readonly ChatUserService _chatUserService;

        public ChatUserServiceTests()
        {
            _mocker = new AutoMocker();

            _chatUserService = _mocker.CreateInstance<ChatUserService>();
        }
        [Fact]
        public void AddUser_WithExistingEmail_ShouldNotAddNewUser()
        {
            //Arrange
            var user = new ActiveUserDto("1", "test@email.com");
            var user2 = new ActiveUserDto("2", "test@email.com");

            //Act
            _chatUserService.AddUser(user2);

            //Assert
            _chatUserService.GetCount().Should().Be(1);
        }


        [Fact(DisplayName = "Should add user and return one")]
        public void AddUser_WithActiveUser_ShouldAddNewUser()
        {
            //Arrange
            var user = new ActiveUserDto("1", "test@email.com");

            //Act
            _chatUserService.AddUser(user);

            //Assert
            _chatUserService.GetCount().Should().Be(1);
        }

        [Fact]
        public void GetByEmail_ShouldReturnCorrectUser()
        {
            //Arrange
            var user = new ActiveUserDto("1", "test@email.com");
            var user2 = new ActiveUserDto("2", "test2@email.com");

            _chatUserService.AddUser(user);
            _chatUserService.AddUser(user2);


            //Act
            var foundUser = _chatUserService.GetByEmail(user.Email);

            //Assert
            foundUser.Should().NotBeNull();
            foundUser.Email.Should().Be(user.Email);
        }
    }
}

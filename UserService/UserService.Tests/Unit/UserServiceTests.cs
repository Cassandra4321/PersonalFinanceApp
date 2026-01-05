using FluentAssertions;
using UserService.Core.Services;
using UserService.Tests.Helpers;

namespace UserService.Tests.Unit
{
    public class UserServiceTests
    {
        [Fact]
        public void CreateUser_ShouldCreateUser_WhenEmailIsUnique()
        {
            // Arrange
            var email = "test@example.com";
            var name = "Test User";

            var repository = new FakeUserRepository();
            var userService = new UserDomainService(repository);

            // Act
            var user = userService.CreateUser(email, name);

            // Assert
            user.Email.Should().Be(email);
            user.Name.Should().Be(name);
        }

        [Fact]
        public void CreateUser_ShouldThrowException_WhenEmailAlreadyExists()
        {
            // Arrange
            var repository = new FakeUserRepository();
            var userService = new UserDomainService(repository);

            userService.CreateUser("test@example.com", "User One");

            // Act
            Action act = () => userService.CreateUser("test@example.com", "User Two");

            // Assert
            act.Should()
                .Throw<InvalidOperationException>()
                .WithMessage("User with this email already exists");
        }

        [Fact]
        public void CreateUser_ShouldUseRepository_ToCheckIfEmailExists()
        {
            // Arrange
            var repository = new FakeUserRepository();
            var userService = new UserDomainService(repository);

            // Act
            userService.CreateUser("test@example.com", "Test User");

            // Assert
            repository.WasEmailChecked.Should().BeTrue();
        }
    }
}

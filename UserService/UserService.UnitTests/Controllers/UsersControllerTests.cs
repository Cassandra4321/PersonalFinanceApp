using BuildingBlocks.Contracts;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UserService.API.Controllers;
using UserService.Contracts.Users;
using UserService.Core.Abstractions;
using UserService.Core.Users;

namespace UserService.UnitTests.Controllers
{
    public sealed class UsersControllerTests
    {
        private readonly Mock<IUserRepository> _mockRepository;
        private readonly Mock<IPublishEndpoint> _mockPublishEndpoint;
        private readonly UsersController _controller;

        public UsersControllerTests()
        {
            _mockRepository = new Mock<IUserRepository>();
            _mockPublishEndpoint = new Mock<IPublishEndpoint>();
            _controller = new UsersController(_mockRepository.Object, _mockPublishEndpoint.Object);
        }

        [Fact]
        public async Task CreateUser_ValidRequest_Returns201Created()
        {
            _mockRepository
                .Setup(r => r.EmailExistsAsync(It.IsAny<Email>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var request = new CreateUserRequest
            {
                Email = "test@test.com",
                FirstName = "Test",
                LastName = "Testsson",
            };

            var result = await _controller.CreateUser(request, CancellationToken.None);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var response = Assert.IsType<UserResponse>(createdResult.Value);
            Assert.Equal("test@test.com", response.Email);
            Assert.Equal("Test", response.FirstName);
            Assert.Equal("Testsson", response.LastName);
        }

        [Fact]
        public async Task CreateUser_EmptyEmail_Returns400BadRequest()
        {
            var request = new CreateUserRequest
            {
                Email = "",
                FirstName = "Test",
                LastName = "Testsson",
            };

            var result = await _controller.CreateUser(request, CancellationToken.None);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task CreateUser_EmptyFirstName_Returns400BadRequest()
        {
            var request = new CreateUserRequest
            {
                Email = "test@test.com",
                FirstName = "",
                LastName = "Testsson",
            };

            var result = await _controller.CreateUser(request, CancellationToken.None);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task CreateUser_EmptyLastName_Returns400BadRequest()
        {
            var request = new CreateUserRequest
            {
                Email = "test@test.com",
                FirstName = "Test",
                LastName = "",
            };

            var result = await _controller.CreateUser(request, CancellationToken.None);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task CreateUser_ExistingEmail_Returns409Conflict()
        {
            _mockRepository
                .Setup(r => r.EmailExistsAsync(It.IsAny<Email>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var request = new CreateUserRequest
            {
                Email = "test@test.com",
                FirstName = "Test",
                LastName = "Testsson",
            };

            var result = await _controller.CreateUser(request, CancellationToken.None);

            Assert.IsType<ConflictObjectResult>(result.Result);
        }

        [Fact]
        public async Task CreateUser_ValidRequest_PublishesUserCreatedEvent()
        {
            _mockRepository
                .Setup(r => r.EmailExistsAsync(It.IsAny<Email>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var request = new CreateUserRequest
            {
                Email = "test@test.com",
                FirstName = "Test",
                LastName = "Testsson",
            };

            await _controller.CreateUser(request, CancellationToken.None);

            _mockPublishEndpoint.Verify(
                p => p.Publish(It.IsAny<UserCreatedEvent>(), It.IsAny<CancellationToken>()),
                Times.Once
            );
        }

        [Fact]
        public async Task GetUser_ExistingUser_Returns200Ok()
        {
            var userId = Guid.NewGuid();
            var user = User.Create(Email.From("test@test.com"), "Test", "Testsson");

            _mockRepository
                .Setup(r => r.GetByIdAsync(It.IsAny<UserId>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            var result = await _controller.GetUser(userId, CancellationToken.None);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<UserResponse>(okResult.Value);
            Assert.Equal("test@test.com", response.Email);
        }

        [Fact]
        public async Task GetUser_NonExistingUser_Returns404NotFound()
        {
            _mockRepository
                .Setup(r => r.GetByIdAsync(It.IsAny<UserId>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);

            var result = await _controller.GetUser(Guid.NewGuid(), CancellationToken.None);

            Assert.IsType<NotFoundResult>(result.Result);
        }
    }
}

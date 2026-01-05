using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UserService.Core.Models;
using UserService.Infrastructure.Persistence;
using UserService.Infrastructure.Repositories;

namespace UserService.Tests.Integration
{
    public class UserRepositoryIntegrationTests
    {
        [Fact]
        public void EmailExists_ShouldReturnTrue_WhenEmailExistsInDatabase()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<UserDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new UserDbContext(options);
            var repository = new UserRepository(context);

            var user = new AppUser("test@example.com", "Test User");

            // Act
            repository.Add(user);
            var exists = repository.EmailExists("test@example.com");

            // Assert
            exists.Should().BeTrue();
        }
    }
}

using UserService.Core.Abstractions;
using UserService.Core.Users;

namespace UserService.Infrastructure.Persistence
{
    public sealed class InMemoryUserRepository : IUserRepository
    {
        private static readonly List<User> Users = new();

        public Task<bool> EmailExistsAsync(
            Email email,
            CancellationToken cancellationToken = default
        )
        {
            var exists = Users.Any(u => u.Email.Equals(email));
            return Task.FromResult(exists);
        }

        public Task AddAsync(User user, CancellationToken cancellationToken = default)
        {
            Users.Add(user);
            return Task.CompletedTask;
        }
    }
}

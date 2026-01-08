using Microsoft.EntityFrameworkCore;
using UserService.Core.Abstractions;
using UserService.Core.Users;
using UserService.Infrastructure.Persistence;

namespace UserService.Infrastructure.Repositories
{
    public sealed class UserRepository : IUserRepository
    {
        private readonly UserDbContext _dbContext;

        public UserRepository(UserDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(User user, CancellationToken cancellationToken)
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> EmailExistsAsync(Email email, CancellationToken cancellationToken)
        {
            return await _dbContext.Users.AnyAsync(u => u.Email == email, cancellationToken);
        }
    }
}

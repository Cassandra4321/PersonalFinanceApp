using UserService.Core.Users;

namespace UserService.Core.Abstractions
{
    public interface IUserRepository
    {
        Task<bool> EmailExistsAsync(Email email, CancellationToken cancellationToken = default);
        Task AddAsync(User user, CancellationToken cancellationToken = default);
    }
}

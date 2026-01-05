using UserService.Core.Interfaces;
using UserService.Core.Models;
using UserService.Infrastructure.Persistence;

namespace UserService.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext _context;

        public UserRepository(UserDbContext context)
        {
            _context = context;
        }

        public bool EmailExists(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }

        public void Add(AppUser user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }
    }
}

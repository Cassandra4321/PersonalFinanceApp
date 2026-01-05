using UserService.Core.Interfaces;
using UserService.Core.Models;

namespace UserService.Tests.Helpers
{
    public class FakeUserRepository : IUserRepository
    {
        private readonly List<AppUser> _users = new();

        public bool WasEmailChecked { get; private set; }

        public bool EmailExists(string email)
        {
            WasEmailChecked = true;
            return _users.Any(u => u.Email == email);
        }

        public void Add(AppUser user)
        {
            _users.Add(user);
        }
    }
}

using UserService.Core.Models;

namespace UserService.Core.Interfaces
{
    public interface IUserRepository
    {
        void Add(AppUser user);
        bool EmailExists(string email);
    }
}

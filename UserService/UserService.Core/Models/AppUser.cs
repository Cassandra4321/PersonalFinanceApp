namespace UserService.Core.Models
{
    public class AppUser
    {
        public string Email { get; set; }
        public string Name { get; set; }

        public AppUser() { }

        public AppUser(string email, string name)
        {
            Email = email;
            Name = name;
        }
    }
}

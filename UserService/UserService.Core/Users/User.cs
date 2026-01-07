using UserService.Core.Common;

namespace UserService.Core.Users
{
    public sealed class User : Entity<UserId>
    {
        public Email Email { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }

        private User() { }

        private User(UserId id, Email email, string firstName, string lastName)
        {
            Id = id;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
        }

        public static User Create(Email email, string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("First name cannot be empty.", nameof(firstName));

            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Last name cannot be empty.", nameof(lastName));

            return new User(UserId.New(), email, firstName.Trim(), lastName.Trim());
        }
    }
}

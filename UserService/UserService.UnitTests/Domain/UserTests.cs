using UserService.Core.Users;

namespace UserService.UnitTests.Domain
{
    public sealed class UserTests
    {
        [Fact]
        public void Create_ValidData_ReturnsUser()
        {
            var email = Email.From("test@test.com");

            var user = User.Create(email, "Test", "Testsson");

            Assert.Equal("test@test.com", user.Email.Value);
            Assert.Equal("Test", user.FirstName);
            Assert.Equal("Testsson", user.LastName);
            Assert.NotEqual(Guid.Empty, user.Id.Value);
        }

        [Fact]
        public void Create_EmptyFirstName_ThrowsArgumentException()
        {
            var email = Email.From("test@test.com");

            Assert.Throws<ArgumentException>(() => User.Create(email, "", "Testsson"));
        }

        [Fact]
        public void Create_EmptyLastName_ThrowsArgumentException()
        {
            var email = Email.From("test@test.com");

            Assert.Throws<ArgumentException>(() => User.Create(email, "Test", ""));
        }

        [Fact]
        public void Create_WhitespaceFirstName_ThrowsArgumentException()
        {
            var email = Email.From("test@test.com");

            Assert.Throws<ArgumentException>(() => User.Create(email, "   ", "Testsson"));
        }

        [Fact]
        public void Create_NameWithWhitespace_TrimmsName()
        {
            var email = Email.From("test@test.com");

            var user = User.Create(email, "  Test  ", "  Testsson  ");

            Assert.Equal("Test", user.FirstName);
            Assert.Equal("Testsson", user.LastName);
        }

        [Fact]
        public void Create_TwoUsers_HaveDifferentIds()
        {
            var email1 = Email.From("test1@test.com");
            var email2 = Email.From("test2@test.com");

            var user1 = User.Create(email1, "Test", "Testsson");
            var user2 = User.Create(email2, "Test", "Testsson");

            Assert.NotEqual(user1.Id, user2.Id);
        }
    }
}

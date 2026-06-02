using UserService.Core.Users;

namespace UserService.UnitTests.Domain
{
    public sealed class EmailTests
    {
        [Fact]
        public void From_ValidEmail_ReturnsEmail()
        {
            var email = Email.From("test@test.com");

            Assert.Equal("test@test.com", email.Value);
        }

        [Fact]
        public void From_ValidEmail_ConvertsToLowerCase()
        {
            var email = Email.From("TEST@TEST.COM");

            Assert.Equal("test@test.com", email.Value);
        }

        [Fact]
        public void From_InvalidEmail_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => Email.From("not-an-email"));
        }

        [Fact]
        public void From_EmptyEmail_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => Email.From(""));
        }

        [Fact]
        public void From_NullEmail_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => Email.From(null!));
        }

        [Theory]
        [InlineData("test@test.com", "test@test.com", true)]
        [InlineData("test@test.com", "other@test.com", false)]
        public void Equals_TwoEmails_ReturnsExpectedResult(
            string first,
            string second,
            bool expectedResult
        )
        {
            var email1 = Email.From(first);
            var email2 = Email.From(second);

            Assert.Equal(expectedResult, email1.Equals(email2));
        }
    }
}

using UserService.Core.Users;

namespace UserService.UnitTests.Domain
{
    public sealed class UserIdTests
    {
        [Fact]
        public void New_CreatesUserIdWithNonEmptyGuid()
        {
            var userId = UserId.New();

            Assert.NotEqual(Guid.Empty, userId.Value);
        }

        [Fact]
        public void From_ValidGuid_ReturnsUserId()
        {
            var guid = Guid.NewGuid();

            var userId = UserId.From(guid);

            Assert.Equal(guid, userId.Value);
        }

        [Fact]
        public void From_EmptyGuid_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => UserId.From(Guid.Empty));
        }

        [Fact]
        public void New_TwoUserIds_AreNotEqual()
        {
            var userId1 = UserId.New();
            var userId2 = UserId.New();

            Assert.NotEqual(userId1, userId2);
        }

        [Fact]
        public void From_SameGuid_AreEqual()
        {
            var guid = Guid.NewGuid();

            var userId1 = UserId.From(guid);
            var userId2 = UserId.From(guid);

            Assert.Equal(userId1, userId2);
        }
    }
}

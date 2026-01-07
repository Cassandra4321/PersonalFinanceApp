using UserService.Core.Common;

namespace UserService.Core.Users
{
    public sealed class UserId : ValueObject
    {
        public Guid Value { get; }

        private UserId(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException("UserId cannot be an empty GUID.", nameof(value));
            }
            Value = value;
        }

        public static UserId New()
        {
            return new UserId(Guid.NewGuid());
        }

        public static UserId From(Guid value)
        {
            return new UserId(value);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public override string ToString() => Value.ToString();
    }
}

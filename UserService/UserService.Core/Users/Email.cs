using System.Text.RegularExpressions;
using UserService.Core.Common;

namespace UserService.Core.Users
{
    public sealed class Email : ValueObject
    {
        private static readonly Regex EmailRegex = new(
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            RegexOptions.Compiled
        );

        public string Value { get; }

        private Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Email cannot be null or empty.", nameof(value));
            }
            if (!EmailRegex.IsMatch(value))
            {
                throw new ArgumentException("Invalid email format.", nameof(value));
            }
            Value = value.ToLowerInvariant();
        }

        public static Email From(string value) => new(value);

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public override string ToString() => Value;
    }
}

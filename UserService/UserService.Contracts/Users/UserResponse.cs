namespace UserService.Contracts.Users
{
    public sealed class UserResponse
    {
        public Guid Id { get; init; }
        public string Email { get; init; } = default!;
        public string FirstName { get; init; } = default!;
        public string LastName { get; init; } = default!;
    }
}

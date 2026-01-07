namespace UserService.Contracts.Users
{
    public sealed class CreateUserRequest
    {
        public string Email { get; init; } = default!;
        public string FirstName { get; init; } = default!;
        public string LastName { get; init; } = default!;
    }
}

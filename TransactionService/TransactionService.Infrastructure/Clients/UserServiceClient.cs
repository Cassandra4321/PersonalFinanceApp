namespace TransactionService.Infrastructure.Clients;

public sealed class UserServiceClient
{
    private readonly HttpClient _httpClient;

    public UserServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> UserExistsAsync(Guid userId, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"/api/users/{userId}", cancellationToken);

        return response.IsSuccessStatusCode;
    }
}

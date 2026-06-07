using System.Net;
using System.Net.Http.Json;
using UserService.Contracts.Users;

namespace UserService.IntegrationTests;

public sealed class UsersControllerIntegrationTests
    : IClassFixture<UserServiceWebApplicationFactory>
{
    private readonly HttpClient _client;

    public UsersControllerIntegrationTests(UserServiceWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateUser_ValidRequest_Returns201Created()
    {
        var request = new CreateUserRequest
        {
            Email = "test@test.com",
            FirstName = "Test",
            LastName = "Testsson",
        };

        var response = await _client.PostAsJsonAsync("/api/users", request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task CreateUser_ValidRequest_ReturnsCorrectData()
    {
        var request = new CreateUserRequest
        {
            Email = "test2@test.com",
            FirstName = "Test",
            LastName = "Testsson",
        };

        var response = await _client.PostAsJsonAsync("/api/users", request);
        var user = await response.Content.ReadFromJsonAsync<UserResponse>();

        Assert.NotNull(user);
        Assert.Equal("test2@test.com", user.Email);
        Assert.Equal("Test", user.FirstName);
        Assert.Equal("Testsson", user.LastName);
        Assert.NotEqual(Guid.Empty, user.Id);
    }

    [Fact]
    public async Task CreateUser_DuplicateEmail_Returns409Conflict()
    {
        var request = new CreateUserRequest
        {
            Email = "duplicate@test.com",
            FirstName = "Test",
            LastName = "Testsson",
        };

        await _client.PostAsJsonAsync("/api/users", request);
        var response = await _client.PostAsJsonAsync("/api/users", request);

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    [Fact]
    public async Task CreateUser_EmptyEmail_Returns400BadRequest()
    {
        var request = new CreateUserRequest
        {
            Email = "",
            FirstName = "Test",
            LastName = "Testsson",
        };

        var response = await _client.PostAsJsonAsync("/api/users", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetUser_ExistingUser_Returns200Ok()
    {
        var createRequest = new CreateUserRequest
        {
            Email = "getuser@test.com",
            FirstName = "Test",
            LastName = "Testsson",
        };

        var createResponse = await _client.PostAsJsonAsync("/api/users", createRequest);
        var createdUser = await createResponse.Content.ReadFromJsonAsync<UserResponse>();

        var getResponse = await _client.GetAsync($"/api/users/{createdUser!.Id}");

        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
    }

    [Fact]
    public async Task GetUser_NonExistingUser_Returns404NotFound()
    {
        var response = await _client.GetAsync($"/api/users/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}

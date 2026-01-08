using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UserService.Core.Abstractions;
using UserService.Infrastructure.Persistence;
using UserService.Infrastructure.Repositories;

namespace UserService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<UserDbContext>(options =>
            options.UseInMemoryDatabase("UserServiceDb")
        );

        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}

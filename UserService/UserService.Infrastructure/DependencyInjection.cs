using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserService.Core.Abstractions;
using UserService.Infrastructure.Persistence;
using UserService.Infrastructure.Repositories;

namespace UserService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var connectionString = configuration.GetConnectionString("UserServiceDb");
        var useInMemory = string.IsNullOrEmpty(connectionString);

        services.AddDbContext<UserDbContext>(options =>
        {
            if (useInMemory)
                options.UseInMemoryDatabase("UserServiceTestDb");
            else
                options.UseSqlServer(connectionString);
        });

        services.AddScoped<IUserRepository, UserRepository>();
        return services;
    }
}

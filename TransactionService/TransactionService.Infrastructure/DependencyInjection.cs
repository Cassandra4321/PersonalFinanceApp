using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TransactionService.Core.Abstractions;
using TransactionService.Infrastructure.Clients;
using TransactionService.Infrastructure.Persistence;
using TransactionService.Infrastructure.Repositories;

namespace TransactionService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.AddDbContext<TransactionDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("TransactionServiceDb"))
            );

            services.AddScoped<ITransactionRepository, TransactionRepository>();

            var userServiceUrl = configuration["Services:UserService"];

            services.AddHttpClient<UserServiceClient>(client =>
            {
                client.BaseAddress = new Uri(userServiceUrl!);
            });

            return services;
        }
    }
}

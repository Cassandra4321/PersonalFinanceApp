using BudgetService.Core.Abstractions;
using BudgetService.Infrastructure.Persistence;
using BudgetService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BudgetService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.AddDbContext<BudgetDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("BudgetServiceDb"))
            );
            services.AddScoped<IBudgetRepository, BudgetRepository>();
            return services;
        }
    }
}

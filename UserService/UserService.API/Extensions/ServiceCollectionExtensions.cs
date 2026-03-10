using Microsoft.Extensions.DependencyInjection;
using UserService.Core.Abstractions;
using UserService.Infrastructure.Persistence;

namespace UserService.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            return services;
        }
    }
}

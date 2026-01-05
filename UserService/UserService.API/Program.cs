using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using UserService.Core.Interfaces;
using UserService.Core.Services;
using UserService.Infrastructure.Persistence;
using UserService.Infrastructure.Repositories;

namespace UserService.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "UserService API", Version = "v1" });
            });

            // Database & Repositories
            builder.Services.AddDbContext<UserDbContext>(options =>
                options.UseInMemoryDatabase("UserDb_API")
            );
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<UserDomainService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment()) { }

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}

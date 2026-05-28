using BudgetService.API.Consumers;
using BudgetService.Infrastructure;
using BudgetService.Infrastructure.Persistence;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace BudgetService.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Controllers
            builder.Services.AddControllers();

            builder.Services.AddDbContext<BudgetDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("BudgetDb"));
            });

            // OpenAPI
            builder.Services.AddOpenApi();
            builder.Services.AddEndpointsApiExplorer();

            // Swagger UI
            builder.Services.AddSwaggerGen();

            // Infrastructure
            builder.Services.AddInfrastructure(builder.Configuration);

            // Masstransit
            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumer<UserCreatedConsumer>();
                x.AddConsumer<TransactionCreatedConsumer>();

                x.UsingRabbitMq(
                    (context, cfg) =>
                    {
                        cfg.Host(
                            "localhost",
                            "/",
                            h =>
                            {
                                h.Username("guest");
                                h.Password("guest");
                            }
                        );

                        cfg.ReceiveEndpoint(
                            "budget-user-created-queue",
                            e =>
                            {
                                e.ConfigureConsumer<UserCreatedConsumer>(context);
                            }
                        );
                        cfg.ReceiveEndpoint(
                            "budget-transaction-created-queue",
                            e =>
                            {
                                e.ConfigureConsumer<TransactionCreatedConsumer>(context);
                            }
                        );
                    }
                );
            });

            var app = builder.Build();

            // HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}

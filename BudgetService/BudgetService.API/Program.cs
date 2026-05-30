using BudgetService.API.Consumers;
using BudgetService.Infrastructure;
using MassTransit;

namespace BudgetService.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Controllers
            builder.Services.AddControllers();

            // OpenAPI
            builder.Services.AddOpenApi();
            builder.Services.AddEndpointsApiExplorer();

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

            // Swagger
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();

                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "User Service API v1");
                    options.RoutePrefix = "swagger";
                });
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}

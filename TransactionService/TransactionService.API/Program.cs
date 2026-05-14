using MassTransit;
using TransactionService.API.Consumers;
using TransactionService.Infrastructure;

namespace TransactionService.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Controllers
            builder.Services.AddControllers();

            // OpenAPI spec
            builder.Services.AddOpenApi();
            builder.Services.AddEndpointsApiExplorer();

            // Swagger UI
            builder.Services.AddSwaggerGen();

            // Infrastructure
            builder.Services.AddInfrastructure(builder.Configuration);

            // MassTransit
            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumer<UserCreatedConsumer>();

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
                            "user-created-event-queue",
                            e =>
                            {
                                e.ConfigureConsumer<UserCreatedConsumer>(context);
                            }
                        );
                    }
                );
            });

            builder.Logging.AddConsole();

            var app = builder.Build();

            var busControl = app.Services.GetRequiredService<IBusControl>();
            Console.WriteLine("MassTransit bus resolved successfully.");

            // HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();

                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Transaction Service API");
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

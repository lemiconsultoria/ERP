
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace ERP.Gateway
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            var ASPNETCORE_ENVIRONMENT = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            builder.Configuration.AddJsonFile($"ocelot.{ASPNETCORE_ENVIRONMENT?.ToLower()}.json", optional: false, reloadOnChange: true);


            builder.Services.AddOcelot(builder.Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.MapControllers();

            await app.UseOcelot();

            app.MapGet("/info", () => "0.0.1");

            app.Run();
        }
    }
}
using ERP.Core.Helpers;
using ERP.Core.Middleware;
using ERP.Identity.IoC;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace ERP.Identity.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.RegisterIoC();

            builder.Configuration.RegisterLogHelper();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = Assembly.GetExecutingAssembly().GetName().Name,
                    Version = "v1",
                    Description = "Servico responsavel pela geracao de token para autenticacao e utilizacao nos outros servicos",
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);                
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment() || Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Container")
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.UseMiddleware(typeof(GlobalErrorHandlingMiddleware));

            app.Run();
        }
    }
}
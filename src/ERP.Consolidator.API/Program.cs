using ERP.Consolidator.IoC;
using ERP.Core.Helpers;
using ERP.Core.Middleware;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace ERP.Consolidator.API
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

            builder.Services.SchedulerJobs(builder.Configuration);
            builder.Services.RegisterJwtAuth(builder.Configuration);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = Assembly.GetExecutingAssembly().GetName().Name,
                    Version = "v1",
                    Description = "Servico responsavel pela tratamento dos dados da mensageria de lancamentos e pela consolidacao dos dados para posterior exibicao de relatorios. Os dados sao consolidados diariamente as 00:00 e 12:00",
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer'[space] and then your token in the text input below.",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                          {
                              Reference = new OpenApiReference
                              {
                                  Type = ReferenceType.SecurityScheme,
                                  Id = "Bearer"
                              }
                          },
                         new string[] {}
                    }
                });
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
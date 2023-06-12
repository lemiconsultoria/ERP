using ERP.Report.Application.Queries;
using ERP.Report.Domain.Interfaces.Repositories;
using ERP.Report.Infra.Data.Context;
using ERP.Report.Infra.Data.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ERP.Report.IoC
{
    public static class ApplicationIoCModule
    {
        public static void RegisterIoC(this IServiceCollection services)
        {
            //data
            services.AddTransient<IEntryBalanceRepository, EntryBalanceRepository>();

            //queries
            services.AddScoped<IEntryBalanceQueries, EntryBalanceQueries>();

            services.AddTransient<MongoDBContext>();
        }

        public static void RegisterJwtAuth(this IServiceCollection services, IConfiguration configuration)
        {
            var secretKeyJWT = configuration.GetSection("JWT")?.GetSection("SecretKey").Value ?? "daf72ddc4316404696a0fd142b1df3a7";

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKeyJWT)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }
    }
}
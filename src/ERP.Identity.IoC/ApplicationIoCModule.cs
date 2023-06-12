using ERP.Identity.Application.Services;
using ERP.Identity.Domain.Interfaces;
using ERP.Identity.Infra.Data.Context;
using ERP.Identity.Infra.Data.Repositories;
using ERP.Identity.Infra.Util.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace ERP.Identity.IoC
{
    public static class ApplicationIoCModule
    {
        public static void RegisterIoC(this IServiceCollection services)
        {
            //data
            services.AddScoped<IUserRepository, UserRepository>();

            //services
            services.AddScoped<IUserService, UserService>();

            //context
            services.AddScoped<FakeContext>();

            //helpers
            services.AddScoped<JwtAuthHelper>();
        }
    }
}
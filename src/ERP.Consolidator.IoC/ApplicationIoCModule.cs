using ERP.Consolidator.Application.Commands;
using ERP.Consolidator.Application.Services;
using ERP.Consolidator.Domain.Commands.EntryBalance;
using ERP.Consolidator.Domain.Interfaces.Repositories;
using ERP.Consolidator.Infra.Data.Context;
using ERP.Consolidator.Infra.Data.Repositories;
using ERP.Consolidator.Infra.Jobs.Jobs;
using ERP.Consolidator.Infra.Messaging;
using ERP.Core.Interfaces;
using ERP.Domain.Events;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Quartz;
using System.Text;

namespace ERP.Consolidator.IoC
{
    public static class ApplicationIoCModule
    {
        public static void RegisterIoC(this IServiceCollection services)
        {
            //data
            services.AddTransient<IEntryCreditRepository, EntryCreditRepository>();
            services.AddTransient<IEntryDebitRepository, EntryDebitRepository>();
            services.AddTransient<IEntryBalanceRepository, EntryBalanceRepository>();
            services.AddTransient<IDateToProcessRepository, DateToProcessRepository>();

            //commandHandlers
            services.AddScoped<ICommandHandler<ConsolidateCommand, ConsolidateCommandResult>, DailyConsolidationCommandHandler>();

            //Messaging
            services.AddTransient<IEventConsumer<CreditEntryEvent, Guid>, CreditEntryEventConsumer>();
            services.AddTransient<IEventConsumer<DebitEntryEvent, Guid>, DebitEntryEventConsumer>();

            services.AddHostedService<ConsumerSubscriptionsService>();

            services.AddTransient<MongoDBContext>();
        }

        public static void SchedulerJobs(this IServiceCollection services, IConfiguration configuration)
        {
            var jobsExpressionConsolidate = configuration.GetSection("Jobs").GetSection("JobsExpressionConsolidate").Value ?? "* * 0/12 ? * * *";
            services.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionJobFactory();

                var jobKey = new JobKey("Consolidate");
                q.AddJob<ConsolidateJob>(opts => opts.WithIdentity(jobKey));

                q.AddTrigger(opts => opts
                    .ForJob(jobKey)
                    .WithIdentity("ConsolidateJob-trigger")
                    .WithCronSchedule(jobsExpressionConsolidate)
                );
            });
            services.AddQuartzServer(q => q.WaitForJobsToComplete = true);
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
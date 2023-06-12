using ERP.Core.Interfaces;
using ERP.Crud.Application.Commands;
using ERP.Crud.Application.Queries;
using ERP.Crud.Domain.Commands.EntryCredit;
using ERP.Crud.Domain.Commands.EntryCredit.Results;
using ERP.Crud.Domain.Commands.EntryCredit.Validators;
using ERP.Crud.Domain.Commands.EntryDebit;
using ERP.Crud.Domain.Commands.EntryDebit.Results;
using ERP.Crud.Domain.Commands.EntryDebit.Validators;
using ERP.Crud.Domain.Interfaces.Repositories;
using ERP.Crud.Infra.Data.Context;
using ERP.Crud.Infra.Data.Repositories;
using ERP.Crud.Infra.Messaging;
using ERP.Domain.Events;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ERP.Crud.IoC
{
    public static class ApplicationIoCModule
    {
        public static void RegisterIoC(this IServiceCollection services)
        {
            //data
            services.AddScoped<IEntryCreditRepository, EntryCreditRepository>();
            services.AddScoped<IEntryDebitRepository, EntryDebitRepository>();

            //validators
            services.AddScoped<IValidator<CreateEntryCreditCommand>, CreateEntryCreditCommandValidator>();
            services.AddScoped<IValidator<UpdateEntryCreditCommand>, UpdateEntryCreditCommandValidator>();
            services.AddScoped<IValidator<DeleteEntryCreditCommand>, DeleteEntryCreditCommandValidator>();

            services.AddScoped<IValidator<CreateEntryDebitCommand>, CreateEntryDebitCommandValidator>();
            services.AddScoped<IValidator<UpdateEntryDebitCommand>, UpdateEntryDebitCommandValidator>();
            services.AddScoped<IValidator<DeleteEntryDebitCommand>, DeleteEntryDebitCommandValidator>();

            //commandHandlers
            services.AddScoped<ICommandHandler<CreateEntryCreditCommand, EntryCreditCommandResultBase>, CreateEntryCreditCommandHandler>();
            services.AddScoped<ICommandHandler<UpdateEntryCreditCommand, EntryCreditCommandResultBase>, UpdateEntryCreditCommandHandler>();
            services.AddScoped<ICommandHandler<DeleteEntryCreditCommand, DeleteEntryCreditCommandResult>, DeleteEntryCreditCommandHandler>();

            services.AddScoped<ICommandHandler<CreateEntryDebitCommand, EntryDebitCommandResultBase>, CreateEntryDebitCommandHandler>();
            services.AddScoped<ICommandHandler<UpdateEntryDebitCommand, EntryDebitCommandResultBase>, UpdateEntryDebitCommandHandler>();
            services.AddScoped<ICommandHandler<DeleteEntryDebitCommand, DeleteEntryDebitCommandResult>, DeleteEntryDebitCommandHandler>();

            //queries
            services.AddScoped<IEntryCreditQueries, EntryCreditQueries>();
            services.AddScoped<IEntryDebitQueries, EntryDebitQueries>();

            //messaging

            services.AddScoped<IEventPublisher<CreditEntryEvent>, CreditEntryEventPublisher<CreditEntryEvent>>();
            services.AddScoped<IEventPublisher<DebitEntryEvent>, DebitEntryEventPublisher<DebitEntryEvent>>();

            /*
            services.AddTransient<IEventConsumer<BookEvent, Guid>, BookEventConsumer>();
            services.AddTransient<IEventConsumer<DeleteBookEvent, Guid>, DeleteBookEventConsumer>();
            services.AddScoped<IConsumerSubscriptions, ConsumerSubscriptions>();

            /*
            //elasticsearch
            services.AddTransient<IElasticContextProvider, ElasticContextProvider>();
            services.AddTransient<IElasticConfigurationService, ElasticConfigurationService>();
            */
        }

        public static void RegisterDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connection = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<MySQLContext>(options =>
            {
                options.UseMySql(connection, new MySqlServerVersion(new Version(8, 0, 33)));
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            
        }

        public static void RunMigrations(this IServiceProvider serviceScopeFactory)
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var dataContext = scope.ServiceProvider.GetRequiredService<MySQLContext>();
                dataContext.Database.Migrate();
            }
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
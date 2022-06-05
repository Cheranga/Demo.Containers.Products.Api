using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Demo.Containers.Products.Api.Extensions;
using Demo.Containers.Products.Api.Features.GetProductById;
using Demo.Containers.Products.Api.Features.GetProductById.V1;
using Demo.Containers.Products.Api.Infrastructure.DataAccess;
using Demo.Containers.Products.Api.Shared;
using FluentValidation;
using MediatR;
using Microsoft.ApplicationInsights.Extensibility;
using Serilog;
using Serilog.Events;

namespace Demo.Containers.Products.Api;

public class Bootstrapper
{
    public static void RegisterServices(WebApplicationBuilder builder, ConfigurationManager configurationManager)
    {
        var services = builder.Services;

        services.AddMediatR(typeof(Bootstrapper).Assembly);
        services.AddValidatorsFromAssembly(typeof(ModelValidatorBase<>).Assembly);

        RegisterAzureClients(builder);
        RegisterConfigurations(services, configurationManager);
        RegisterResponseGenerators(services);
        RegisterBehaviours(services);
        RegisterLogging(builder, configurationManager);
    }

    private static void RegisterLogging(WebApplicationBuilder builder, ConfigurationManager configurationManager)
    {
        builder.Host.UseSerilog((context, configuration) =>
        {
            if (context.HostingEnvironment.IsDevelopment())
            {
                configuration.MinimumLevel.Debug()
                    .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}");
            }
            else
            {
                // var appInsightsKey = configurationManager["ApplicationInsights:InstrumentationKey"] ?? "";
                builder.Services.AddApplicationInsightsTelemetry();
                builder.Services.AddServiceProfiler();

                configuration.MinimumLevel.Debug()
                    .WriteTo.ApplicationInsights(TelemetryConfiguration.CreateDefault(), TelemetryConverter.Traces, LogEventLevel.Debug);
            }
        });
    }

    private static void RegisterConfigurations(IServiceCollection services, ConfigurationManager configurationManager)
    {
         services.AddScoped(_ =>
         {
             var databaseConfig = configurationManager.GetSection(nameof(DatabaseConfig)).Get<DatabaseConfig>();
             return databaseConfig;
         });
    }

    private static void RegisterBehaviours(IServiceCollection services)
    {
        services.RegisterBehaviours<GetProductByIdRequest, GetProductByIdResponse>();
        services.RegisterBehaviours<GetProductByIdQuery, ProductDataModel>();
    }

    private static void RegisterResponseGenerators(IServiceCollection services)
    {
        services.AddScoped<IResponseGenerator<GetProductByIdResponse>, GetProductByIdResponseGenerator>();
    }

    private static void RegisterAzureClients(WebApplicationBuilder builder)
    {
        if (builder.Environment.IsDevelopment())
        {
            return;
        }
        
        var keyVaultName = builder.Configuration["KeyVaultName"];
        var keyVaultUri = new Uri($"https://{keyVaultName}.vault.azure.net/");

        var secretClient = new SecretClient(keyVaultUri, new DefaultAzureCredential(new DefaultAzureCredentialOptions
        {
            ExcludeManagedIdentityCredential = false
        }));

        builder.Configuration.AddAzureKeyVault(secretClient, new AzureKeyVaultConfigurationOptions
        {
            ReloadInterval = TimeSpan.FromSeconds(30)
        });
    }
}
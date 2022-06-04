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

        RegisterConfigurations(services, configurationManager);
        RegisterResponseGenerators(services);
        RegisterBehaviours(services);
        RegisterLogging(builder);
    }

    private static void RegisterLogging(WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, configuration) =>
        {
            if (string.Equals("local", context.HostingEnvironment.EnvironmentName, StringComparison.OrdinalIgnoreCase))
            {
                configuration.MinimumLevel.Debug()
                    .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}");
            }
            else
            {
                configuration.MinimumLevel.Debug()
                    .WriteTo.ApplicationInsights(TelemetryConfiguration.CreateDefault(), TelemetryConverter.Traces, LogEventLevel.Debug);
            }
        });
    }

    private static void RegisterConfigurations(IServiceCollection services, ConfigurationManager configurationManager)
    {
        var connectionString = configurationManager["DatabaseConfig__ConnectionString"] ?? "";
        services.AddSingleton(new DatabaseConfig
        {
            ConnectionString = connectionString
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
}
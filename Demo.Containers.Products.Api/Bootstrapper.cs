using Demo.Containers.Products.Api.Extensions;
using Demo.Containers.Products.Api.Features.GetProductById;
using Demo.Containers.Products.Api.Infrastructure.DataAccess;
using Demo.Containers.Products.Api.Shared;
using FluentValidation;
using MediatR;

namespace Demo.Containers.Products.Api;

public class Bootstrapper
{
    public static void RegisterServices(IServiceCollection services, ConfigurationManager configurationManager)
    {
        services.AddMediatR(typeof(Bootstrapper).Assembly);
        services.AddValidatorsFromAssembly(typeof(ModelValidatorBase<>).Assembly);

        RegisterConfigurations(services, configurationManager);
        RegisterResponseGenerators(services);
        RegisterBehaviours(services);
        RegisterLogging(services, configurationManager);
    }

    private static void RegisterLogging(IServiceCollection services, ConfigurationManager configurationManager)
    {
        services.AddLogging(builder =>
        {
            var isLocal = string.Equals(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"), "Local", StringComparison.OrdinalIgnoreCase);
            if (isLocal)
            {
                builder.AddConsole();
            }
            else
            {
                var instrumentationKey = Environment.GetEnvironmentVariable("APPINSIGHTS_INSTRUMENTATIONKEY");
                services.AddApplicationInsightsTelemetry(instrumentationKey);
            }
        });
    }

    private static void RegisterConfigurations(IServiceCollection services, ConfigurationManager configurationManager)
    {
        var connectionString = configurationManager["DatabaseConfig:ConnectionString"] ?? "";
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
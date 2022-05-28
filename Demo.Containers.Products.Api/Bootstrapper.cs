using System.ComponentModel.DataAnnotations.Schema;
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
    }

    private static void RegisterConfigurations(IServiceCollection services, ConfigurationManager configurationManager)
    {
        var databaseConfig = configurationManager.GetSection(nameof(DatabaseConfig)).Get<DatabaseConfig>();
        services.AddSingleton(databaseConfig);
    }

    private static void RegisterBehaviours(IServiceCollection services)
    {
        services.AddTransient<IPipelineBehavior<GetProductByIdRequest, Result<GetProductByIdResponse>>, PerformanceBehaviour<GetProductByIdRequest, GetProductByIdResponse>>();
        services.AddTransient<IPipelineBehavior<GetProductByIdRequest, Result<GetProductByIdResponse>>, ValidationBehaviour<GetProductByIdRequest, GetProductByIdResponse>>();
        
        services.AddTransient<IPipelineBehavior<GetProductByIdQuery, Result<ProductDataModel>>, PerformanceBehaviour<GetProductByIdQuery, ProductDataModel>>();
        services.AddTransient<IPipelineBehavior<GetProductByIdQuery, Result<ProductDataModel>>, ValidationBehaviour<GetProductByIdQuery, ProductDataModel>>();
    }

    private static void RegisterResponseGenerators(IServiceCollection services)
    {
        services.AddScoped<IResponseGenerator<GetProductByIdResponse>, GetProductByIdResponseGenerator>();
    }
}
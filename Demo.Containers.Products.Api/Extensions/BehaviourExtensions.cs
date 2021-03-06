using Demo.Containers.Products.Api.Shared;
using MediatR;

namespace Demo.Containers.Products.Api.Extensions;

public static class BehaviourExtensions
{
    public static void RegisterBehaviours<TInput, TOutput>(this IServiceCollection services, ServiceLifetime lifeTime = ServiceLifetime.Transient)
        where TInput : IValidatable, IRequest<Result<TOutput>>
        where TOutput : class
    {
        services.AddTransient<IPipelineBehavior<TInput, Result<TOutput>>, PerformanceBehaviour<TInput, TOutput>>();
        services.AddTransient<IPipelineBehavior<TInput, Result<TOutput>>, ValidationBehaviour<TInput, TOutput>>();
    }
}
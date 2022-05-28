using System.Diagnostics;
using MediatR;

namespace Demo.Containers.Products.Api.Shared;

public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, Result<TResponse>>
    where TRequest : IValidatable, IRequest<Result<TResponse>>
    where TResponse : class
{
    private readonly ILogger<PerformanceBehaviour<TRequest, TResponse>> _logger;

    public PerformanceBehaviour(ILogger<PerformanceBehaviour<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<Result<TResponse>> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<Result<TResponse>> next)
    {
        var stopWatch = new Stopwatch();

        _logger.LogInformation("{CorrelationId} {Operation} started", request.CorrelationId, typeof(TRequest).Name);
        stopWatch.Start();
        var operation = await next();

        stopWatch.Stop();
        _logger.LogInformation("{CorrelationId} {Operation} operation finished. Time taken {TimeTaken}ms", request.CorrelationId, typeof(TRequest).Name, stopWatch.ElapsedMilliseconds);

        return operation;
    }
}
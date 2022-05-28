using FluentValidation;
using MediatR;

namespace Demo.Containers.Products.Api.Shared;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, Result<TResponse>> where TRequest : IValidatable, IRequest<Result<TResponse>>
{
    private readonly IValidator<TRequest>? _validator;
    private readonly ILogger<ValidationBehaviour<TRequest, TResponse>> _logger;

    public ValidationBehaviour(IValidator<TRequest> validator, ILogger<ValidationBehaviour<TRequest, TResponse>> logger)
    {
        _validator = validator;
        _logger = logger;
    }
    
    public async Task<Result<TResponse>> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<Result<TResponse>> next)
    {
        if (_validator == null)
        {
            return await next();
        }
        
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid)
        {
            _logger.LogInformation("{CorrelationId} {Operation} validation successful", request.CorrelationId, typeof(TRequest).Name);
            return await next();
        }
        
        _logger.LogError("invalid {Request} received", typeof(TRequest).Name);
        return Result<TResponse>.Failure(ErrorCodes.InvalidRequest, validationResult);
    }
}
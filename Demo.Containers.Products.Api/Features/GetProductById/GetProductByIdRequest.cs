using Demo.Containers.Products.Api.Shared;
using FluentValidation;
using MediatR;

namespace Demo.Containers.Products.Api.Features.GetProductById;

public class GetProductByIdRequest : IRequest<Result<GetProductByIdResponse>>, IValidatable
{
    public GetProductByIdRequest(string correlationId, string productId)
    {
        ProductId = productId;
        CorrelationId = correlationId;
    }

    public string ProductId { get; }

    public string CorrelationId { get; set; }
}

public class GetProductByIdRequestValidator : ModelValidatorBase<GetProductByIdRequest>
{
    public GetProductByIdRequestValidator()
    {
        RuleFor(x => x.CorrelationId).NotNull().NotEmpty().WithMessage("correlationId is required");
        RuleFor(x => x.ProductId).NotNull().NotEmpty().WithMessage("productId is required");
    }
}

public class GetProductByIdRequestHandler : IRequestHandler<GetProductByIdRequest, Result<GetProductByIdResponse>>
{
    private readonly ILogger<GetProductByIdRequestHandler> _logger;
    private readonly IMediator _mediator;

    public GetProductByIdRequestHandler(IMediator mediator, ILogger<GetProductByIdRequestHandler> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<Result<GetProductByIdResponse>> Handle(GetProductByIdRequest request, CancellationToken cancellationToken)
    {
        var query = new GetProductByIdQuery(request.CorrelationId, request.ProductId);
        var getProductByIdOperation = await _mediator.Send(query, cancellationToken);

        if (!getProductByIdOperation.Status) return Result<GetProductByIdResponse>.Failure(getProductByIdOperation.ErrorCode, getProductByIdOperation.ValidationResult);

        var product = getProductByIdOperation.Data;
        if (product == null) return Result<GetProductByIdResponse>.Failure(ErrorCodes.ProductNotFound, ErrorMessages.ProductNotFound);

        var response = new GetProductByIdResponse
        {
            Id = product.Id,
            Category = product.Category,
            Name = product.Name,
            Price = product.Price
        };

        return Result<GetProductByIdResponse>.Success(response);
    }
}
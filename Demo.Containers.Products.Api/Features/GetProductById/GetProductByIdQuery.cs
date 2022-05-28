using Demo.Containers.Products.Api.Infrastructure.DataAccess;
using Demo.Containers.Products.Api.Shared;
using FluentValidation;
using MediatR;

namespace Demo.Containers.Products.Api.Features.GetProductById;

public class GetProductByIdQuery : IValidatable, IQuery, IRequest<Result<ProductDataModel>>
{
    public string ProductId { get; set; }
    public string CorrelationId { get; set; }
}

public class GetProductByIdQueryValidator : ModelValidatorBase<GetProductByIdQuery>
{
    public GetProductByIdQueryValidator()
    {
        RuleFor(x => x.CorrelationId).NotNull().NotEmpty();
        RuleFor(x => x.ProductId).NotNull().NotEmpty();
    }
}

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Result<ProductDataModel>>
{
    private readonly DatabaseConfig _databaseConfig;

    public GetProductByIdQueryHandler(DatabaseConfig databaseConfig)
    {
        _databaseConfig = databaseConfig;
    }

    public async Task<Result<ProductDataModel>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromSeconds(2));
        return Result<ProductDataModel>.Success(new ProductDataModel
        {
            Id = "1",
            Category = "Gardening",
            Name = "Showel",
            Price = 35.50m
        });
    }
}
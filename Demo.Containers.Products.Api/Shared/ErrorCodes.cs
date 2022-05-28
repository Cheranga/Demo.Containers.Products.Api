namespace Demo.Containers.Products.Api.Shared;

public class ErrorCodes
{
    public const string InvalidRequest = nameof(InvalidRequest);
    public const string ProductNotFound = nameof(ProductNotFound);
    public const string InternalError = nameof(InternalError);
}

public class ErrorMessages
{
    public const string InvalidRequest = "invalid request";
    public const string ProductNotFound = "product not found";
    public const string InternalError = "internal error occurred";
}

public class Headers
{
    public const string CorrelationId = nameof(CorrelationId);
}
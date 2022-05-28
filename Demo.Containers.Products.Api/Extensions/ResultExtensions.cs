using Demo.Containers.Products.Api.Shared;

namespace Demo.Containers.Products.Api.Extensions;

public static class ResultExtensions
{
    public static Result<T> ToSuccess<T>(this T model)
    {
        return Result<T>.Success(model);
    }
}
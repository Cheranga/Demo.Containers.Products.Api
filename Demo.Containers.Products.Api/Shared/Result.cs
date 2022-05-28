using FluentValidation.Results;

namespace Demo.Containers.Products.Api.Shared;

public class Result<TData> where TData : class
{
    private Result(string errorCode, ValidationResult validationResult)
    {
        ErrorCode = errorCode;
        ValidationResult = validationResult;
    }

    private Result(TData data)
    {
        Data = data;
    }

    public string ErrorCode { get; } = string.Empty;
    public ValidationResult ValidationResult { get; } = new();
    public TData? Data { get; }

    public bool Status => string.IsNullOrEmpty(ErrorCode);

    public static Result<TData> Failure(string errorCode, string errorMessage)
    {
        var failure = new ValidationFailure(errorCode, errorMessage)
        {
            ErrorCode = errorCode
        };
        return Failure(errorCode, new ValidationResult(new[] {failure}));
    }

    public static Result<TData> Failure(string errorCode, ValidationResult validationResult)
    {
        return new Result<TData>(errorCode, validationResult);
    }

    public static Result<TData> Success(TData data)
    {
        return new Result<TData>(data);
    }
}
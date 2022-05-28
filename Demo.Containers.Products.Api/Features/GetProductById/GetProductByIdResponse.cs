using System.Net;
using Demo.Containers.Products.Api.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Containers.Products.Api.Features.GetProductById;

public class GetProductByIdResponse
{
    public string? Id { get; set; }
    public string? Category { get; set; }
    public string? Name { get; set; }
    public decimal Price { get; set; }
}

public class GetProductByIdResponseGenerator : IResponseGenerator<GetProductByIdResponse>
{
    public IActionResult GetResponse(Result<GetProductByIdResponse> operation)
    {
        if (operation.Status) return new OkObjectResult(operation.Data);

        return GetErrorResponse(operation);
    }

    private IActionResult GetErrorResponse(Result<GetProductByIdResponse> operation)
    {
        ErrorResponse errorResponse;
        switch (operation.ErrorCode)
        {
            case ErrorCodes.InvalidRequest:
                errorResponse = new ErrorResponse
                {
                    ErrorCode = ErrorCodes.InvalidRequest,
                    ErrorMessage = ErrorMessages.InvalidRequest,
                    Errors = operation.ValidationResult.Errors.Select(x => new ErrorData
                    {
                        ErrorCode = x.PropertyName,
                        ErrorMessage = x.ErrorMessage
                    }).ToList()
                };

                return new BadRequestObjectResult(errorResponse);

            case ErrorCodes.ProductNotFound:
                errorResponse = new ErrorResponse
                {
                    ErrorCode = ErrorCodes.ProductNotFound,
                    ErrorMessage = ErrorMessages.ProductNotFound
                };
                return new ObjectResult(errorResponse)
                {
                    StatusCode = (int) HttpStatusCode.NotFound
                };

            default:
                errorResponse = new ErrorResponse
                {
                    ErrorCode = ErrorCodes.InternalError,
                    ErrorMessage = ErrorMessages.InternalError,
                    Errors = operation.ValidationResult.Errors.Select(x => new ErrorData
                    {
                        ErrorCode = x.ErrorCode,
                        ErrorMessage = x.ErrorMessage
                    }).ToList()
                };

                return new ObjectResult(errorResponse)
                {
                    StatusCode = (int) HttpStatusCode.InternalServerError
                };
        }
    }
}
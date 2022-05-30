using System.ComponentModel.DataAnnotations;
using Demo.Containers.Products.Api.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Containers.Products.Api.Features.GetProductById.V1;

public class GetProductByIdController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IResponseGenerator<GetProductByIdResponse> _responseGenerator;

    public GetProductByIdController(IMediator mediator, IResponseGenerator<GetProductByIdResponse> responseGenerator)
    {
        _mediator = mediator;
        _responseGenerator = responseGenerator;
    }

    [HttpGet("api/v1/products/{productId}")]
    public async Task<IActionResult> GetProductById([FromHeader] [Required] string correlationId, [FromRoute] string productId)
    {
        var getProductByIdRequest = new GetProductByIdRequest(correlationId, productId);

        var getProductOperation = await _mediator.Send(getProductByIdRequest);

        var response = _responseGenerator.GetResponse(getProductOperation);
        return response;
    }
}
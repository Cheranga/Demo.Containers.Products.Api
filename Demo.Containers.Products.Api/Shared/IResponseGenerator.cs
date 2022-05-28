using Microsoft.AspNetCore.Mvc;

namespace Demo.Containers.Products.Api.Shared;

public interface IResponseGenerator<T>
{
    IActionResult GetResponse(Result<T> operation);
}
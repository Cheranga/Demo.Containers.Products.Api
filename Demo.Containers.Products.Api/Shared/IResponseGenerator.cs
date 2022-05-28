using Microsoft.AspNetCore.Mvc;

namespace Demo.Containers.Products.Api.Shared;

public interface IResponseGenerator<T> where T : class
{
    IActionResult GetResponse(Result<T> operation);
}
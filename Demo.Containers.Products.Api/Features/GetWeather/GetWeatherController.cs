using Microsoft.AspNetCore.Mvc;

namespace Demo.Containers.Products.Api.Features.GetWeather;

public class GetWeatherController : ControllerBase
{
    private static readonly string[] Summaries =
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<GetWeatherController> _logger;

    public GetWeatherController(ILogger<GetWeatherController> logger)
    {
        _logger = logger;
    }

    [HttpGet("api/weather")]
    public IActionResult Get()
    {
        _logger.LogInformation("getting weather report");

        var weatherResults = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();

        return Ok(weatherResults);
    }
}
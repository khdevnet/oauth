using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SPA.Controllers
{
    public class ForecastPolicy
    {
        public class ReaderResourcePermission
        {
            public const string PolicyName = "Forecast reader";
            public const string UserForecastReadPermission = "forecasts.read";
            public const string ForecastReadScope = "forecasts.read";
        }

        public class ExternalReaderResourcePermission
        {
            public const string PolicyName = "Forecast external reader";
            public const string UserForecastReadPermission = "forecasts.read";
            public const string ForecastExternlReadScope = "forecasts.loadexternal";
        }
    }

    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [Authorize(Policy = ForecastPolicy.ReaderResourcePermission.PolicyName)]
        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 2).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [Authorize(Policy = ForecastPolicy.ExternalReaderResourcePermission.PolicyName)]
        [HttpGet("external")]
        public IEnumerable<WeatherForecast> GetExternal()
        {
            return Enumerable.Range(1, 2).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
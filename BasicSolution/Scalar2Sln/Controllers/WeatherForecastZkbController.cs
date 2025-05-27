using MediatR;
using Microsoft.AspNetCore.Mvc;
using Scalar2Sln_Application.Handler.WeatherForecasts.Queries.GetWeatherForecasts;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Scalar2Sln.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastZkbController  : ControllerBase
    {       

        private readonly ILogger<WeatherForecastZkbController> _logger;
        private readonly IMediator _mediator;
        public WeatherForecastZkbController(ILogger<WeatherForecastZkbController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet(Name = "GetWeatherForecastZkb")]
        public async Task<Ok<List<Scalar2Sln_Application.Handler.WeatherForecasts.Queries.GetWeatherForecasts.WeatherForecast>>> GetZkb()
        {
            var forecasts = await _mediator.Send(new GetWeatherForecastsQuery());

            var sd=  forecasts.ToList();   
            return TypedResults.Ok(sd);

          
        }
    }
}

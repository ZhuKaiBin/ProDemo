using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.HttpResults;
using Scalar2Sln.Infrastructure;
using Scalar2Sln_Application.Handler.WeatherForecasts.Queries.GetWeatherForecasts;
namespace Scalar2Sln.Endpoints
{
    public class WeatherForecast:EndpointGroupBase
    {
         public override void Map(WebApplication app)
         {
             app.MapGroup(this)
                 //.RequireAuthorization()
                 .MapGet(GetWeatherForecasts);
         }

    public async Task<Ok<IEnumerable<Scalar2Sln_Application.Handler.WeatherForecasts.Queries.GetWeatherForecasts.WeatherForecast>>> GetWeatherForecasts(ISender sender)
    {
        var forecasts = await sender.Send(new GetWeatherForecastsQuery());

        return TypedResults.Ok(forecasts);
    }
}
}

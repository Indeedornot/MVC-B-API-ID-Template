using Refit;
using SharedProject.Api;
using SharedProject.Models.Api.WeatherEndpoint;

namespace MVCServer.Api;

public interface IWebApi {
  [Get(ApiEndpoints.WeatherEndpoint)]
  Task<WeatherResponse?> GetWeatherForecast();
}
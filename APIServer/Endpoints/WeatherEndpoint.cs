using FastEndpoints;
using SharedProject.Api;
using SharedProject.Models;
using SharedProject.Models.Api.WeatherEndpoint;

namespace APIServer.Endpoints;

public class WeatherEndpoint : EndpointWithoutRequest<WeatherResponse> {
  private static readonly string[] Summaries = {
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
  };

  public override void Configure() {
    this.ResponseCache(60);
    this.Get(ApiEndpoints.WeatherEndpoint);
  }

  public async override Task HandleAsync(CancellationToken ct) {
    var rng = new Random();
    var forecasts = Enumerable.Range(1, 5).Select(
      index => new WeatherForecast {
        Date = DateTime.Now.AddDays(index),
        TemperatureC = rng.Next(-20, 55),
        Summary = Summaries[rng.Next(Summaries.Length)]
      }
    ).ToList();
    await this.SendAsync(new WeatherResponse {Forecasts = forecasts});
  }
}
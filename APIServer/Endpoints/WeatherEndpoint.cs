using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FastEndpoints;
using SharedProject.Api;
using SharedProject.Models;
using SharedProject.Models.Api.WeatherEndpoint;

namespace APIServer.Endpoints {

  [HttpGet(ApiEndpoints.WeatherEndpoint)]
  public class WeatherEndpoint : EndpointWithoutRequest<WeatherResponse> {
    public override async Task HandleAsync(CancellationToken ctx) {
      var rng = new Random();
      var forecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast {
        Date = DateTime.Now.AddDays(index),
        TemperatureC = rng.Next(-20, 55),
        Summary = Summaries[rng.Next(Summaries.Length)]
      }).ToList();
      await SendAsync(new WeatherResponse { Forecasts = forecasts }, cancellation: ctx);
    }
    private static readonly string[] Summaries = new[]{
             "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
  }
}
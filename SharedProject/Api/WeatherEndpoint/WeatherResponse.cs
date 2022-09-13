using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharedProject.Models.Api.WeatherEndpoint {
  public class WeatherResponse {
    public List<WeatherForecast> Forecasts { get; set; } = new();
  }
}
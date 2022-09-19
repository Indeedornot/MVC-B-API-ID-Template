using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCServer.Api;
using MVCServer.Models;
using SharedProject.IdentityServer;
using SharedProject.Models.Api.WeatherEndpoint;

namespace MVCServer.Controllers;

public class HomeController : Controller {
  private readonly IWebApi _webApi;

  public HomeController(IWebApi webApi) {
    _webApi = webApi;
  }

  public async Task<IActionResult> Index() {
    WeatherResponse? weather = null;
    try{
      weather = await _webApi.GetWeatherForecast();
    }
    catch (Exception e){
      Console.WriteLine(e);
    }

    return this.View(weather);
  }

  [Authorize(Roles = Roles.Admin)]
  public IActionResult Privacy() {
    return this.View();
  }

  [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
  public IActionResult Error() {
    return this.View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
  }
}
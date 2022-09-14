using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MVCServer.Api;
using MVCServer.Models;

namespace MVCServer.Controllers;

public class HomeController : Controller {
  private readonly IWebApi _webApi;

  public HomeController(IWebApi webApi) {
    _webApi = webApi;
  }

  public async Task<IActionResult> Index() {
    //ViewData["Username"] = ProfileService.GetProfileDataAsync(new()).Result.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
    // }
    //   var y = response.Claims.ToDictionary(c => c.Type, c => c.Value);
    //   });
    //     Token = token
    //     Address = "https://localhost:5001/connect/userinfo",
    //   var response = await client.GetUserInfoAsync(new UserInfoRequest {
    //   var client = new HttpClient();
    //   var token = await context.GetTokenAsync(CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectParameterNames.AccessToken);
    //   var x = User.Claims.ToDictionary(c => c.Type, c => c.Value);
    // {
    // if (User?.Identity?.IsAuthenticated is true)
    //HttpContext context
    var weather = await _webApi.GetWeatherForecast();
    return this.View(weather);
  }

  public IActionResult Privacy() {
    return this.View();
  }

  [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
  public IActionResult Error() {
    return this.View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
  }
}
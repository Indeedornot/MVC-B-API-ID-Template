using System.Net;
using System.Diagnostics;
using IdentityModel.Client;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using MVCServer.Models;
using Microsoft.AspNetCore.Authorization;

namespace MVCServer.Controllers;

public class HomeController : Controller {

  public HomeController() { }


  public IActionResult Index(HttpContext context) {
    // if (User?.Identity?.IsAuthenticated is true)
    // {
    //   var x = User.Claims.ToDictionary(c => c.Type, c => c.Value);

    //   var token = await context.GetTokenAsync(CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectParameterNames.AccessToken);
    //   var client = new HttpClient();
    //   var response = await client.GetUserInfoAsync(new UserInfoRequest {
    //     Address = "https://localhost:5001/connect/userinfo",
    //     Token = token
    //   });
    //   var y = response.Claims.ToDictionary(c => c.Type, c => c.Value);
    // }

    //ViewData["Username"] = ProfileService.GetProfileDataAsync(new()).Result.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
    return this.View();
    }
  public IActionResult Privacy() {
    return this.View();
    }

  [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
  public IActionResult Error() {
    return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
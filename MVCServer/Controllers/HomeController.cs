using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MVCServer.Models;

namespace MVCServer.Controllers;

public class HomeController : Controller {
  private readonly ILogger<HomeController> _logger;

  public HomeController(ILogger<HomeController> logger) {
    _logger = logger;
  }

  public IActionResult Index() {
    //ViewBag["Username"] = I
    return this.View();
  }

  public IActionResult Privacy() {
    return this.View();
  }

  [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
  public IActionResult Error() {
    return this.View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
  }
}
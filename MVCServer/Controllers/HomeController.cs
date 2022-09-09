using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MVCServer.Models;

namespace MVCServer.Controllers;

public class HomeController : Controller {
  private readonly ILogger<HomeController> _logger;

  public HomeController(ILogger<HomeController> logger) {
    _logger = logger;
  }

  public IActionResult Index()
    => this.View();

  public IActionResult Privacy()
    => this.View();

  [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
  public IActionResult Error()
    => this.View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
}
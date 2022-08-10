using System.Diagnostics;
using ClozerWoods.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClozerWoods.Controllers;
public class MainGateController : Controller {
    private readonly ILogger<MainGateController> _logger;

    public MainGateController(ILogger<MainGateController> logger) {
        _logger = logger;
    }

    [Route("")]
    public IActionResult Index() {
        return View();
    }

    [Route("Error")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

using System.Diagnostics;
using ClozerWoods.Models;
using ClozerWoods.Models.MainGate;
using ClozerWoods.Models.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ClozerWoods.Controllers;
public class MainGateController : Controller {
    private IPageRepository _pageRepo;
    private SharedViewModel _layoutViewModel;

    public MainGateController(IPageRepository pageRepo) {
        _pageRepo = pageRepo;
        _layoutViewModel = new SharedViewModel {
            PublishedPages = _pageRepo.Pages
                             .OrderBy(x => x.Title)
                             .Where(p => p.Published)
        };
    }

    [Route("")]
    public IActionResult Index() {
        return View(_layoutViewModel);
    }

    [Route("Error")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

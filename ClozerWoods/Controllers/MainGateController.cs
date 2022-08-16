using System.Diagnostics;
using ClozerWoods.Models.ViewModels;
using ClozerWoods.Models.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ClozerWoods.Controllers;
public class MainGateController : Controller {
    private readonly IPageRepository _pageRepo;
    private readonly SharedViewModel _layoutViewModel;

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

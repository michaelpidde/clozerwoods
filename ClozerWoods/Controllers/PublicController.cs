using System.Diagnostics;
using ClozerWoods.Models.ViewModels;
using ClozerWoods.Models.Repositories;
using Microsoft.AspNetCore.Mvc;
using ClozerWoods.Models.ViewModels.Public;

namespace ClozerWoods.Controllers;
public class PublicController : Controller {
    private readonly IPageRepository _pageRepo;
    private readonly SharedViewModel _layoutViewModel;

    public PublicController(IPageRepository pageRepo) {
        _pageRepo = pageRepo;
        _layoutViewModel = new SharedViewModel {
            PublishedPages = _pageRepo.GetPublished,
        };
    }

    [Route("")]
    public IActionResult Index() {
        return View(_layoutViewModel);
    }

    [Route("Page/{stub}")]
    public IActionResult Page(string stub) {
        var model = new PageViewModel {
            SelectedPage = _pageRepo.GetByStub(stub),
            PublishedPages = _pageRepo.GetPublished,
        };
        return View(model);
    }

    [Route("Error")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

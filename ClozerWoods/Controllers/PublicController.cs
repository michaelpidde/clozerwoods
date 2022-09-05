using System.Diagnostics;
using ClozerWoods.Models.ViewModels;
using ClozerWoods.Models.Repositories;
using Microsoft.AspNetCore.Mvc;
using ClozerWoods.Models.ViewModels.Public;
using ClozerWoods.Services;

namespace ClozerWoods.Controllers;
public class PublicController : Controller {
    private readonly IConfiguration _config;
    private readonly IGalleryRepository _galleryRepo;
    private readonly IMediaItemRepository _mediaItemRepo;
    private readonly IPageRepository _pageRepo;
    private readonly QuickTagService _quickTagService;

    public PublicController(IConfiguration config, IGalleryRepository galleryRepo, IMediaItemRepository mediaItemRepo, IPageRepository pageRepo) {
        _config = config;
        _galleryRepo = galleryRepo;
        _mediaItemRepo = mediaItemRepo;
        _pageRepo = pageRepo;
        _quickTagService = new QuickTagService(_galleryRepo, _mediaItemRepo, _config["MediaUrl"]);
    }

    [Route("")]
    public IActionResult Index() {
        var home = _pageRepo.GetHome;
        if(home == null) {
            throw new Exception("No published home page configured.");
        }

        return View("Page", new PageViewModel {
            SelectedPage = home,
            PublishedPages = _pageRepo.GetPublished(excludeChildren: true),
            QuickTagService = _quickTagService,
        });
    }

    [Route("Page/{stub}")]
    public IActionResult Page(string stub) {
        var model = new PageViewModel {
            SelectedPage = _pageRepo.GetByStub(stub),
            PublishedPages = _pageRepo.GetPublished(excludeChildren: true),
            QuickTagService = _quickTagService,
        };
        return View(model);
    }

    [Route("Error")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

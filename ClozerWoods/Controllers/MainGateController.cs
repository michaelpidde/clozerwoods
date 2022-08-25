using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ClozerWoods.Models.ViewModels.MainGate;
using ClozerWoods.Models.Entities;
using ClozerWoods.Models.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ClozerWoods.Models.ViewModels;

namespace ClozerWoods.Controllers {
    [Route("maingate")]
    public class MainGateController : Controller {
        private readonly IGalleryRepository _galleryRepo;
        private readonly IMediaItemRepository _mediaItemRepo;
        private readonly IPageRepository _pageRepo;
        private readonly IUserRepository _userRepo;

        public MainGateController(IGalleryRepository galleryRepo,
                                 IMediaItemRepository mediaItemRepo,
                                 IPageRepository pageRepo,
                                 IUserRepository userRepo) {
            _galleryRepo = galleryRepo;
            _mediaItemRepo = mediaItemRepo;
            _pageRepo = pageRepo;
            _userRepo = userRepo;
        }

        [HttpGet("login")]
        public IActionResult Login() {
            return View(new SharedViewModel());
        }

        [HttpPost("login")]
        public async Task<RedirectToActionResult> LoginActionAsync(string username, string password) {
            User? user = null;
            try {
                using var hasher = SHA256.Create();
                byte[] hash = hasher.ComputeHash(Encoding.UTF8.GetBytes(password));
                user = _userRepo.GetUser(username, hash);
            } catch(UserNotFoundException) {
                // TODO: Add message
                return RedirectToAction("login");
            }

            var claims = new List<Claim> {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, "Administrator")
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            return RedirectToAction("dashboard");
        }

        [Authorize]
        [Route("dashboard")]
        public IActionResult Dashboard() {
            return View(new SharedViewModel());
        }

        [Authorize]
        [HttpGet("editpage")]
        public IActionResult EditPage(uint? pageId = null) {
            var selected = new Page();
            if(pageId != null) {
                try {
                    selected = _pageRepo.Get((uint)pageId);
                } catch(PageNotFoundException) {
                    // TODO: Log this or something
                }
            }

            var model = new PageViewModel {
                PageList = GetPagesForSelect(pageId),
                ParentPageList = GetPagesForSelect(null, "* Select"),
                SelectedPage = selected,
            };

            return View(model);
        }

        [Authorize]
        [HttpPost("editpage")]
        public IActionResult EditPageAction(string title, string[] published, string content, uint? pageId = null, uint? parentId = null) {
            Page modified;

            if(pageId == null) {
                modified = _pageRepo.Add(new Page {
                    Title = title,
                    Content = content,
                    ParentId = parentId,
                    Published = published.Any(),
                    Created = DateTime.Now,
                });
                pageId = modified.Id;
            } else {
                modified = _pageRepo.Update(new Page {
                    Title = title,
                    Content = content,
                    ParentId = parentId,
                    Published = published.Any(),
                    Id = (uint)pageId,
                    Updated = DateTime.Now,
                });
            }

            var model = new PageViewModel {
                PageList = GetPagesForSelect(pageId),
                ParentPageList = GetPagesForSelect(parentId, "* Select"),
                SelectedPage = modified,
            };

            return View("EditPage", model);
        }

        [Authorize]
        [HttpGet("editgallery")]
        public IActionResult EditGallery(uint? galleryId = null) {
            var selected = new Gallery();
            if(galleryId != null) {
                try {
                    selected = _galleryRepo.Get((uint)galleryId);
                } catch(GalleryNotFoundException) {
                    // TODO: Log this or something
                }
            }

            var model = new GalleryViewModel {
                GalleryList = GetGalleriesForSelect(galleryId),
                SelectedGallery = selected,
            };

            return View(model);
        }

        [Authorize]
        [HttpPost("editgallery")]
        public IActionResult EditGalleryAction(string title, uint? galleryId = null) {
            Gallery modified;

            if(galleryId == null) {
                modified = _galleryRepo.Add(new Gallery {
                    Title = title,
                    Created = DateTime.Now,
                });
                galleryId = modified.Id;
            } else {
                modified = _galleryRepo.Update(new Gallery {
                    Title = title,
                    Id = (uint)galleryId,
                    Updated = DateTime.Now,
                });
            }

            var model = new GalleryViewModel {
                GalleryList = GetGalleriesForSelect(galleryId),
                SelectedGallery = modified,
            };

            return View("EditGallery", model);
        }

        [Authorize]
        [HttpGet("editmediaitems")]
        public IActionResult EditMediaItems(uint? itemId = null) {
            var selected = new MediaItem();
            if(itemId != null) {
                try {
                    selected = _mediaItemRepo.Get((uint)itemId);
                } catch(MediaItemNotFoundException) {
                    // TODO: Log this or something
                }
            }

            var model = new MediaItemsViewModel {
                MediaItemList = _mediaItemRepo.MediaItems,
                SelectedMediaItem = selected,
            };

            return View(model);
        }

        private IEnumerable<SelectListItem> GetGalleriesForSelect(uint? galleryId = null) {
            var list = _galleryRepo.Galleries
                       .OrderBy(gallery => gallery.Title)
                       .Select(g => new SelectListItem {
                           Value = g.Id.ToString(),
                           Text = g.Title,
                           Selected = g.Id == galleryId,
                       });
            return list.Prepend(new SelectListItem {
                Value = "",
                Text = "* New",
            });
        }

        private IEnumerable<SelectListItem> GetPagesForSelect(uint? pageId = null, string? defaultItemLabel = "* New") {
            var list = _pageRepo.Pages
                       .OrderBy(page => page.Title)
                       .Select(p => new SelectListItem {
                           Value = p.Id.ToString(),
                           Text = p.Title,
                           Selected = p.Id == pageId,
                       });
            return list.Prepend(new SelectListItem {
                Value = "",
                Text = defaultItemLabel,
            });
        }
    }
}

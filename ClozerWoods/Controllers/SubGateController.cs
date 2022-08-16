using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ClozerWoods.Models.ViewModels.SubGate;
using ClozerWoods.Models.Entities;
using ClozerWoods.Models.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ClozerWoods.Models.ViewModels;

namespace ClozerWoods.Controllers {
    [Route("subgate")]
    public class SubGateController : Controller {
        private readonly IGalleryRepository _galleryRepo;
        private readonly IPageRepository _pageRepo;
        private readonly IUserRepository _userRepo;

        public SubGateController(IGalleryRepository galleryRepo,
                                 IPageRepository pageRepo,
                                 IUserRepository userRepo) {
            _galleryRepo = galleryRepo;
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
        public IActionResult EditPage(int? pageId = null) {
            var selected = new Page();
            if(pageId != null) {
                try {
                    selected = _pageRepo.Get((int)pageId);
                } catch(PageNotFoundException) {
                    // TODO: Log this or something
                }
            }

            var model = new PageViewModel {
                PageListItems = GetPagesForSelect(pageId),
                SelectedPage = selected,
                PublishedPages = null,
            };

            return View(model);
        }

        [Authorize]
        [HttpPost("editpage")]
        public IActionResult EditPageAction(string title, string[] published, int? pageId = null) {
            Page modified;

            if(pageId == null) {
                modified = _pageRepo.Add(new Page {
                    Title = title,
                    Published = published.Any(),
                    Created = DateTime.Now,
                });
                pageId = modified.Id;
            } else {
                modified = _pageRepo.Update(new Page {
                    Title = title,
                    Published = published.Any(),
                    Id = (int)pageId,
                    Updated = DateTime.Now,
                });
            }

            var model = new PageViewModel {
                PageListItems = GetPagesForSelect(pageId),
                SelectedPage = modified,
                PublishedPages = null,
            };

            return View("EditPage", model);
        }

        [Authorize]
        [HttpGet("editgallery")]
        public IActionResult EditGallery(int? galleryId = null) {
            var selected = new Gallery();
            if(galleryId != null) {
                try {
                    selected = _galleryRepo.Get((int)galleryId);
                } catch(GalleryNotFoundException) {
                    // TODO: Log this or something
                }
            }

            var model = new GalleryViewModel {
                GalleryListItems = GetGalleriesForSelect(galleryId),
                SelectedGallery = selected,
                PublishedPages = null,
            };

            return View(model);
        }

        [Authorize]
        [HttpPost("editgallery")]
        public IActionResult EditGalleryAction(string title, int? galleryId = null) {
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
                    Id = (int)galleryId,
                    Updated = DateTime.Now,
                });
            }

            var model = new GalleryViewModel {
                GalleryListItems = GetGalleriesForSelect(galleryId),
                SelectedGallery = modified,
                PublishedPages = null,
            };

            return View("EditGallery", model);
        }

        private IEnumerable<SelectListItem> GetGalleriesForSelect(int? galleryId = -1) {
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

        private IEnumerable<SelectListItem> GetPagesForSelect(int? pageId = -1) {
            var list = _pageRepo.Pages
                       .OrderBy(page => page.Title)
                       .Select(p => new SelectListItem {
                           Value = p.Id.ToString(),
                           Text = p.Title,
                           Selected = p.Id == pageId,
                       });
            return list.Prepend(new SelectListItem {
                Value = "",
                Text = "* New",
            });
        }
    }
}

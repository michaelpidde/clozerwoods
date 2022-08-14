using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ClozerWoods.Models.SubGate;
using ClozerWoods.Models.Entities;
using ClozerWoods.Models.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ClozerWoods.Models.MainGate;

namespace ClozerWoods.Controllers {
    [Route("subgate")]
    public class SubGateController : Controller {
        private IUserRepository _userRepo;
        private IPageRepository _pageRepo;

        public SubGateController(IUserRepository userRepo, IPageRepository pageRepo) {
            _userRepo = userRepo;
            _pageRepo = pageRepo;
        }

        [HttpGet("login")]
        public IActionResult Login() {
            return View();
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

            var model = new PagesViewModel {
                PageListItems = GetPagesForSelect(pageId),
                SelectedPage = selected,
                PublishedPages = new List<Page>(),
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
                    Updated = DateTime.Now,
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

            var model = new PagesViewModel {
                PageListItems = GetPagesForSelect(pageId),
                SelectedPage = modified,
                PublishedPages = new List<Page>(),
            };

            return View("EditPage", model);
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

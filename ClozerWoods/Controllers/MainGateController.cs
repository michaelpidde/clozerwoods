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
using ClozerWoods.Models.ViewModels;
using ClozerWoods.Services;

namespace ClozerWoods.Controllers {
    [Route("maingate")]
    public class MainGateController : Controller {
        private readonly IConfiguration _config;
        private readonly IGalleryRepository _galleryRepo;
        private readonly IMediaItemRepository _mediaItemRepo;
        private readonly IPageRepository _pageRepo;
        private readonly IUserRepository _userRepo;
        private readonly MainGateService _service;


        public MainGateController(
            IConfiguration config,
            IGalleryRepository galleryRepo,
            IMediaItemRepository mediaItemRepo,
            IPageRepository pageRepo,
            IUserRepository userRepo
        ) {
            _config = config;
            _galleryRepo = galleryRepo;
            _mediaItemRepo = mediaItemRepo;
            _pageRepo = pageRepo;
            _userRepo = userRepo;
            _service = new MainGateService();
        }



        #region Auth
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
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));

            return RedirectToAction("dashboard");
        }
        #endregion



        #region Dashboard
        [Authorize]
        [Route("dashboard")]
        public IActionResult Dashboard() {
            return View(new SharedViewModel());
        }
        #endregion



        #region Pages
        [Authorize]
        [HttpGet("pages")]
        public IActionResult ListPages(bool modified = false) {
            return View(new ListPagesViewModel {
                PageList = _pageRepo.Pages,
                Modified = modified,
            });
        }

        [Authorize]
        [HttpGet("editpage/{pageId?}")]
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
                ParentPageList = _pageRepo.GetForSelect(null, "* Select"),
                SelectedPage = selected,
            };

            return View(model);
        }



        [Authorize]
        [HttpPost("editpage/{pageId?}")]
        public IActionResult EditPageAction(
            string stub,
            string title,
            string[] published,
            string[] isHome,
            string content,
            uint? pageId = null,
            uint? parentId = null
        ) {
            if(pageId == null) {
                _ = _pageRepo.Add(new Page {
                    Stub = stub,
                    Title = title,
                    Content = content,
                    ParentId = parentId,
                    Published = published.Any(),
                    IsHome = isHome.Any(),
                    Created = DateTime.Now,
                });
            } else {
                _ = _pageRepo.Update(new Page {
                    Stub = stub,
                    Title = title,
                    Content = content,
                    ParentId = parentId,
                    Published = published.Any(),
                    IsHome = isHome.Any(),
                    Id = (uint)pageId,
                    Updated = DateTime.Now,
                });
            }

            return RedirectToAction("ListPages", new { modified = true });
        }
        #endregion



        #region Galleries
        [Authorize]
        [HttpGet("galleries")]
        public IActionResult ListGalleries(bool modified = false) {
            return View(new ListGalleriesViewModel {
                GalleryList = _galleryRepo.Galleries,
                Modified = modified,
            });
        }



        [Authorize]
        [HttpGet("editgallery/{galleryId?}")]
        public IActionResult EditGallery(uint? galleryId = null, bool add = false) {
            var selected = new Gallery();
            if(galleryId != null) {
                try {
                    selected = _galleryRepo.Get((uint)galleryId);
                } catch(GalleryNotFoundException) {
                    // TODO: Log this or something
                }
            }

            return View(new GalleryViewModel {
                SelectedGallery = selected,
                Add = add,
            });
        }



        [Authorize]
        [HttpPost("editgallery/{galleryId?}")]
        public IActionResult EditGalleryAction(string title, uint? galleryId = null) {
            if(galleryId == null) {
                _ = _galleryRepo.Add(new Gallery {
                    Title = title,
                    Created = DateTime.Now,
                });
            } else {
                _ = _galleryRepo.Update(new Gallery {
                    Title = title,
                    Id = (uint)galleryId,
                    Updated = DateTime.Now,
                });
            }

            return RedirectToAction("ListGalleries", new { modified = true });
        }
        #endregion



        #region Media Items
        [Authorize]
        [HttpGet("mediaitems")]
        public IActionResult ListMediaItems(bool modified = false) {
            return View(new ListMediaItemsViewModel {
                MediaUrl = _config["MediaUrl"],
                MediaItemList = _mediaItemRepo.MediaItems,
                Modified = modified,
            });
        }



        [Authorize]
        [HttpGet("editmediaitem/{mediaItemId?}")]
        public IActionResult EditMediaItem(uint? mediaItemId = null, bool add = false) {
            var selected = new MediaItem();
            if(mediaItemId != null) {
                try {
                    selected = _mediaItemRepo.Get((uint)mediaItemId);
                } catch(MediaItemNotFoundException) {
                    // TODO: Log this or something
                }
            }

            return View(new MediaItemsViewModel {
                MediaUrl = _config["MediaUrl"],
                SelectedMediaItem = selected,
                GalleryList = _galleryRepo.GetForSelect(selected.GalleryId, "* Select"),
                Add = add,
            });
        }



        [Authorize]
        [HttpPost("editmediaitem/{mediaItemId?}")]
        public IActionResult EditMediaItemAction(
            bool hasExistingFile,
            uint? galleryId,
            IFormFile? selectedFile,
            string[] forceUniqueName,
            string? title,
            string? description,
            uint? mediaItemId = null
        ) {
            string mediaFolder = _config["MediaFolder"];

            if(!Directory.Exists(mediaFolder)) {
                
                try {
                    Directory.CreateDirectory(mediaFolder);
                } catch(Exception) {
                    // TODO: Handle this gracefully
                    throw new Exception("Media folder does not exist. Unable to create it.");
                }
                
            }

            string fileName;
            MediaItem? saved = null;
            string thumbnail;
            if(hasExistingFile) {
                saved = _mediaItemRepo.Get((uint)mediaItemId!);
            }

            List<string> allowedExtensions = _config.GetSection("ValidMediaExtensions").Get<List<string>>();

            if(selectedFile != null) {
                if(!allowedExtensions.Contains(Path.GetExtension(selectedFile.FileName.ToLower()))) {
                    // TODO: Catch this and handle it gracefully
                    throw new Exception("Invalid media item extension.");
                }

                if(hasExistingFile) {
                    System.IO.File.Delete(mediaFolder + Path.DirectorySeparatorChar + saved!.FileName);
                    System.IO.File.Delete(mediaFolder + Path.DirectorySeparatorChar + saved!.Thumbnail);
                }

                fileName = (forceUniqueName.Any() ? _service.UniqueString() : "") + selectedFile.FileName;
                string fullPath = mediaFolder + Path.DirectorySeparatorChar + fileName;
                using var stream = System.IO.File.Create(fullPath);
                selectedFile.CopyTo(stream);
                thumbnail = _service.GenerateThumbnail(stream, mediaFolder, fileName);
            } else {
                fileName = (saved != null) ? saved.FileName : "";
                thumbnail = (saved != null) ? saved.Thumbnail : "";
            }

            if(mediaItemId == null) {
                _ = _mediaItemRepo.Add(new MediaItem {
                    Title = title,
                    Description = description,
                    FileName = fileName,
                    Thumbnail = thumbnail,
                    GalleryId = galleryId,
                    Created = DateTime.Now,
                });
            } else {
                _ = _mediaItemRepo.Update(new MediaItem {
                    Title = title,
                    Description = description,
                    FileName = fileName,
                    Thumbnail = thumbnail,
                    GalleryId= galleryId,
                    Id = (uint)mediaItemId,
                    Updated = DateTime.Now,
                });
            }

            return RedirectToAction("ListMediaItems", new { modified = true });
        }
        #endregion
    }
}

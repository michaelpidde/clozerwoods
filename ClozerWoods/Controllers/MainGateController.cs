﻿using System.Security.Claims;
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
                PageList = _pageRepo.GetForSelect(pageId),
                ParentPageList = _pageRepo.GetForSelect(null, "* Select"),
                SelectedPage = selected,
            };

            return View(model);
        }



        [Authorize]
        [HttpPost("editpage")]
        public IActionResult EditPageAction(
            string title,
            string[] published,
            string content,
            uint? pageId = null,
            uint? parentId = null
        ) {
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
                PageList = _pageRepo.GetForSelect(pageId),
                ParentPageList = _pageRepo.GetForSelect(parentId, "* Select"),
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
                GalleryList = _galleryRepo.GetForSelect(galleryId),
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
                GalleryList = _galleryRepo.GetForSelect(galleryId),
                SelectedGallery = modified,
            };

            return View("EditGallery", model);
        }



        [Authorize]
        [HttpGet("editmediaitem")]
        public IActionResult EditMediaItem(uint? mediaItemId = null, bool modified = false) {
            var selected = new MediaItem();
            if(mediaItemId != null) {
                try {
                    selected = _mediaItemRepo.Get((uint)mediaItemId);
                } catch(MediaItemNotFoundException) {
                    // TODO: Log this or something
                }
            }

            var model = new MediaItemsViewModel {
                MediaUrl = _config["MediaUrl"],
                MediaItemList = _mediaItemRepo.MediaItems,
                SelectedMediaItem = (mediaItemId == null) ? null : selected,
                GalleryList = _galleryRepo.GetForSelect(selected.GalleryId, "* Select"),
                Modified = modified
            };

            return View(model);
        }



        [Authorize]
        [HttpPost("editmediaitem")]
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

            MediaItem modified;

            if(mediaItemId == null) {
                modified = _mediaItemRepo.Add(new MediaItem {
                    Title = title,
                    Description = description,
                    FileName = fileName,
                    Thumbnail = thumbnail,
                    GalleryId = galleryId,
                    Created = DateTime.Now,
                });
            } else {
                modified = _mediaItemRepo.Update(new MediaItem {
                    Title = title,
                    Description = description,
                    FileName = fileName,
                    Thumbnail = thumbnail,
                    GalleryId= galleryId,
                    Id = (uint)mediaItemId,
                    Updated = DateTime.Now,
                });
            }

            return RedirectToAction("EditMediaItem", new { mediaItemId = modified.Id, modified = true });
        }
    }
}

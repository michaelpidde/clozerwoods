using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ClozerWoods.Models.Entities;
using ClozerWoods.Models.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClozerWoods.Controllers {
    [Route("subgate")]
    public class SubGateController : Controller {
        private IUserRepository _userRepository;

        public SubGateController(IUserRepository userRepository) {
            _userRepository = userRepository;
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
                user = _userRepository.GetUser(username, hash);
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
            return View();
        }
    }
}

using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;
using ClozerWoods.Models.Entities;
using ClozerWoods.Models.Repositories;
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
        public RedirectToActionResult LoginAction(string username, string password) {
            try {
                using var hasher = SHA256.Create();
                byte[] hash = hasher.ComputeHash(Encoding.UTF8.GetBytes(password));
                User? user = _userRepository.GetUser(username, hash);
            } catch(UserNotFoundException) {
                // TODO: Add message
                return RedirectToAction("login");
            }

            // TODO: Set auth cookie
            return RedirectToAction("dashboard");
        }

        [Route("dashboard")]
        public IActionResult Dashboard() {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using RedisSessionDemo.Models;
using System.Text.Json;

namespace RedisSessionDemo.Controllers
{
    public class AccountController : Controller
    {
        // Simulated user database
        private readonly List<(string username, string password, int id, string type, string role)> _users = new()
        {
            ("admin", "admin123", 1, "Internal", "Administrator"),
            ("john", "john123", 2, "Customer", "User"),
            ("sarah", "sarah123", 3, "Customer", "Premium User")
        };

        public IActionResult Login()
        {
            // If already logged in, redirect to dashboard
            var sessionData = HttpContext.Session.GetString("UserSession");
            if (!string.IsNullOrEmpty(sessionData))
            {
                return RedirectToAction("Dashboard");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            // Validate credentials
            var user = _users.FirstOrDefault(u =>
                u.username == model.Username && u.password == model.Password);

            if (user != default)
            {
                // Create session object
                var userSession = new UserSession
                {
                    UserId = user.id,
                    UserName = user.username,
                    UserType = user.type,
                    Role = user.role
                };

                // Serialize and store in Redis via session
                var sessionJson = JsonSerializer.Serialize(userSession);
                HttpContext.Session.SetString("UserSession", sessionJson);

                // Store individual values as well (alternative approach)
                HttpContext.Session.SetInt32("UserId", user.id);
                HttpContext.Session.SetString("UserName", user.username);

                TempData["Message"] = "Login successful! Session stored in Redis.";
                return RedirectToAction("Dashboard");
            }

            ViewBag.Error = "Invalid username or password";
            return View(model);
        }

        public IActionResult Dashboard()
        {
            // Retrieve session from Redis
            var sessionData = HttpContext.Session.GetString("UserSession");

            if (string.IsNullOrEmpty(sessionData))
            {
                return RedirectToAction("Login");
            }

            // Deserialize session data
            var userSession = JsonSerializer.Deserialize<UserSession>(sessionData);
            return View(userSession);
        }

        public IActionResult Logout()
        {
            // Clear session from Redis
            HttpContext.Session.Clear();
            TempData["Message"] = "Logged out successfully. Session removed from Redis.";
            return RedirectToAction("Login");
        }

        public IActionResult CheckRedis()
        {
            // Show what's stored in Redis
            var sessionData = HttpContext.Session.GetString("UserSession");
            var userId = HttpContext.Session.GetInt32("UserId");
            var userName = HttpContext.Session.GetString("UserName");

            ViewBag.SessionJson = sessionData ?? "No session data";
            ViewBag.UserId = userId?.ToString() ?? "Not set";
            ViewBag.UserName = userName ?? "Not set";
            ViewBag.SessionId = HttpContext.Session.Id;

            return View();
        }
    }
}

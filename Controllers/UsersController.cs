using Microsoft.AspNetCore.Mvc;
using StarEvents.Models;
using StarEvents.Services;
using MongoDB.Driver;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace StarEvents.Controllers
{
    public class UsersController : Controller
    {
        private readonly MongoDBContext _context;

        public UsersController(MongoDBContext context)
        {
            _context = context;
        }

        // GET: /Users/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // GET: /Users/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Users/Register
        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Check if user already exists
                    var existingUser = await _context.Users
                        .Find(u => u.Email == user.Email || u.Username == user.Username)
                        .FirstOrDefaultAsync();

                    if (existingUser != null)
                    {
                        ModelState.AddModelError("", "User already exists with this email or username");
                        return View(user);
                    }

                    // Hash password
                    user.PasswordHash = HashPassword(user.PasswordHash);
                    user.DateCreated = DateTime.UtcNow;
                    user.Role = "Customer"; // Default role

                    await _context.Users.InsertOneAsync(user);

                    TempData["SuccessMessage"] = "Registration successful! Please login.";
                    return RedirectToAction("Login");
                }

                return View(user);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred during registration. Please try again.");
                return View(user);
            }
        }

        // POST: /Users/Login
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    ModelState.AddModelError("", "Please enter both email and password");
                    return View();
                }

                // Find user by email
                var user = await _context.Users
                    .Find(u => u.Email == email)
                    .FirstOrDefaultAsync();

                if (user == null || !VerifyPassword(password, user.PasswordHash))
                {
                    ModelState.AddModelError("", "Invalid email or password");
                    return View();
                }

                // TODO: Implement session or cookie-based authentication
                HttpContext.Session.SetString("UserId", user.Id);
                HttpContext.Session.SetString("UserEmail", user.Email);
                HttpContext.Session.SetString("UserRole", user.Role);

                TempData["SuccessMessage"] = $"Welcome back, {user.Username}!";

                // Redirect based on role
                return user.Role switch
                {
                    "Admin" => RedirectToAction("Index", "Admin"),
                    "Organizer" => RedirectToAction("Index", "Organizer"),
                    _ => RedirectToAction("Index", "Home")
                };
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred during login. Please try again.");
                return View();
            }
        }

        // GET: /Users/Logout
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["SuccessMessage"] = "You have been logged out successfully.";
            return RedirectToAction("Index", "Home");
        }

        // GET: /Users/Profile
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login");
            }

            var user = await _context.Users
                .Find(u => u.Id == userId)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Login");
            }

            return View(user);
        }

        // POST: /Users/Profile
        [HttpPost]
        public async Task<IActionResult> Profile(User updatedUser)
        {
            try
            {
                var userId = HttpContext.Session.GetString("UserId");
                if (string.IsNullOrEmpty(userId))
                {
                    return RedirectToAction("Login");
                }

                if (ModelState.IsValid)
                {
                    var filter = Builders<User>.Filter.Eq(u => u.Id, userId);
                    var update = Builders<User>.Update
                        .Set(u => u.Username, updatedUser.Username)
                        .Set(u => u.Email, updatedUser.Email)
                        .Set(u => u.Profile, updatedUser.Profile);

                    await _context.Users.UpdateOneAsync(filter, update);

                    TempData["SuccessMessage"] = "Profile updated successfully!";
                    return RedirectToAction("Profile");
                }

                return View(updatedUser);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while updating profile. Please try again.");
                return View(updatedUser);
            }
        }

        // Password hashing methods
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        private bool VerifyPassword(string inputPassword, string storedHash)
        {
            var hashOfInput = HashPassword(inputPassword);
            return hashOfInput == storedHash;
        }
    }
}
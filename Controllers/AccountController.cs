using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TEST2.Models;
using System.Linq;

namespace TEST2.Controllers
{
    public class AccountController : Controller
    {
        private readonly YourDbContext _context;
        private readonly ILogger<AccountController> _logger;

        public AccountController(YourDbContext context, ILogger<AccountController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult LoginSignup()
        {
            return View(new loginsignup());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.LogError(error.ErrorMessage);
                }

                ViewBag.ActiveTab = "login";
                return View("LoginSignup", model);
            }

            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Username == model.Username);

                if (user == null || !VerifyPassword(model.Password, user.PasswordHash))
                {
                    ModelState.AddModelError(string.Empty, "Invalid username or password.");
                    ViewBag.ActiveTab = "login";
                    return View("LoginSignup", model);
                }

                // Successful login logic
                return RedirectToAction("Home", "Account");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during login: {ex.Message}");
                ModelState.AddModelError(string.Empty, "An unexpected error occurred.");
            }

            ViewBag.ActiveTab = "login";
            return View("LoginSignup", model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Signup(SignupModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ActiveTab = "signup";
                return View("LoginSignup", model);
            }
            try
            {
                // Hash the password
                var passwordHash = HashPassword(model.Password);

                // Create a new user object
                var newUser = new Users
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    PasswordHash = passwordHash,
                    Username = model.Username,
                    Phone = model.Phone ?? "Not provided",  // Provide default values if optional
                    Address = model.Address ?? "Not provided",
                    City = model.City ?? "Not provided",
                    PostalCode = model.PostalCode ?? "Not provided",
                    UserType = "Customer"  // Assuming a default user type
                };

                // Add the new user to the database
                _context.Users.Add(newUser);
                _context.SaveChanges();

                // Redirect to the Home page after signup
                return RedirectToAction("Home", "Account");
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                _logger.LogError($"Error saving user: {ex.Message}");
                ModelState.AddModelError(string.Empty, "An error occurred while saving your data.");
            }

            ViewBag.ActiveTab = "signup";
            return View("LoginSignup", model);
        }

        // Google Login action
        [HttpGet("LoginWithGoogle")]
        public IActionResult LoginWithGoogle()
        {
            var redirectUrl = Url.Action("GoogleResponse", "Account");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        // Google Signup action
        [HttpGet("SignupWithGoogle")]
        public IActionResult SignupWithGoogle()
        {
            var redirectUrl = Url.Action("GoogleResponse", "Account");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        // Google response callback after authentication
        [HttpGet]
        public async Task<IActionResult> GoogleResponse()
        {
            // Log start of the method
            _logger.LogInformation("Entering GoogleResponse method");

            // Attempt to get authentication info
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (result == null || !result.Succeeded)
            {
                _logger.LogWarning("Google authentication failed or returned null info.");
                return RedirectToAction("LoginSignup");
            }

            // Retrieve user info from Google claims
            var email = result.Principal.FindFirstValue(ClaimTypes.Email) ?? "unknown";
            var firstName = result.Principal.FindFirstValue(ClaimTypes.GivenName) ?? "Unknown";
            var lastName = result.Principal.FindFirstValue(ClaimTypes.Surname) ?? "User";

            _logger.LogInformation($"Retrieved Google user info - Email: {email}, First Name: {firstName}, Last Name: {lastName}");

            // Check if the user already exists in the database
            var existingUser = _context.Users.FirstOrDefault(u => u.Email == email);
            if (existingUser != null)
            {
                _logger.LogInformation("User already exists in the database. Redirecting to homepage.");
                return RedirectToAction("Index", "Home");
            }

            // New Google user - create a new record in the database
            var newUser = new Users
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                UserType = "Customer",
                Address = "Not provided", // Placeholder values for fields not provided by Google
                City = "Not provided",
                PostalCode = "Not provided",
                Phone = "Not provided",
                PasswordHash = "GoogleOAuth", // Placeholder for password field since it’s not used with OAuth
                Username = firstName + lastName

            };

            _context.Users.Add(newUser);
            _logger.LogInformation("Attempting to save new user to database.");

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("User saved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error saving user to the database: {ex.Message}");
                ModelState.AddModelError(string.Empty, "An error occurred while saving the user data.");
                return RedirectToAction("LoginSignup");
            }

            // Redirect to the homepage after successful registration
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Home()
        {
            return View(); // Ensure the "Views/Account/Home.cshtml" exists
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
        private bool VerifyPassword(string inputPassword, string storedHashedPassword)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                // Hash the input password
                var hashedInput = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(inputPassword)));

                // Compare the hashed input password with the stored hashed password
                return hashedInput == storedHashedPassword;
            }
        }
    }
}
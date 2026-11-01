using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LoginApp.Data;
using LoginApp.Models;

namespace LoginApp.Controllers
{
    // Handles user authentication features such as
    // logging in, registering, and logging out.
    public class AccountController : Controller
    {
        // Database context used to access the Users table.
        private readonly ApplicationDbContext _context;

        // Password hasher used to securely hash and verify passwords.
        private readonly PasswordHasher<User> _passwordHasher = new();

        // Constructor receives the database context through dependency injection.
        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Displays the login page.
        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        // Processes the submitted login form.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            // If the submitted form data is invalid,
            // redisplay the form with validation messages.
            if (!ModelState.IsValid)
                return View(model);

            // Look up the user by username.
            //.FirstOrDefaultAsync is an Entity Framework Core method that searches for the first record matching a condition.
            //u.Username is the username stored in the database.
            //model.Username is the username submitted by the user through the login form.
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == model.Username);//lambda operator. It separates the input parameters on the left from the expression or code on the right.

            // User not found.
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return View(model);
            }

            // Verify that the entered password matches the stored hash.
            //The method compares the entered password (model.Password) against the stored password hash (user.PasswordHash).
            //the result is a  PasswordVerificationResult datatype
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);

            // Password is incorrect.
            if (result == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return View(model);
            }

            // Create a list of claims that represent the authenticated user.
            //A claim in ASP.NET Core is a piece of information about an authenticated user
            //User submits login form -> Application verifies password -> Application creates claims -> Application creates an authentication cookie
            var claims = new List<Claim>
            {
                // Stores the username.
                new(ClaimTypes.Name, user.Username),

                // Stores the user's email.
                new(ClaimTypes.Email, user.Email),

                // Stores the user's database ID.
                new("UserId", user.Id.ToString())
            };

            // Create an identity using cookie authentication.
            var claimsIdentity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            // Sign the user in by creating an authentication cookie.
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties
                {
                    // Keep the user logged in if "Remember Me" is checked.
                    IsPersistent = model.RememberMe,

                    // Cookie expires after 30 days if remembered,
                    // otherwise after 1 day.
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(
                        model.RememberMe ? 30 : 1)
                });

            // Redirect to the home page after successful login.
            return RedirectToAction("Index", "Home");
        }

        // Displays the registration page.
        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        // Processes the submitted registration form.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            // If validation fails, redisplay the form.
            if (!ModelState.IsValid)
                return View(model);

            // Check if the username or email already exists.
            bool exists = await _context.Users.AnyAsync(u =>
                u.Username == model.Username || u.Email == model.Email);

            if (exists)
            {
                ModelState.AddModelError(string.Empty, "Username or email already in use.");
                return View(model);
            }

            // Create a new user object.
            var user = new User
            {
                Username = model.Username,
                Email = model.Email
            };

            // Hash the password before storing it in the database.
            user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);

            // Save the new user.
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Redirect to the login page.
            return RedirectToAction("Login");
        }

        // Logs the user out.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // Remove the authentication cookie.
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            // Return to the login page.
            return RedirectToAction("Login");
        }
    }
}
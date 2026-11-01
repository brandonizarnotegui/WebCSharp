// Brings in MVC types: Controller, IActionResult, [HttpGet], [HttpPost], etc.
using Microsoft.AspNetCore.Mvc;
// Brings in our SignupModel class.
using ValidationDemo.Models;

// File-scoped namespace: everything below belongs to this namespace.
namespace ValidationDemo.Controllers;

// Inheriting from Controller gives access to View(), ModelState, TempData, etc.
// The name "AccountController" maps to the URL segment /Account.
public class AccountController : Controller
{
    // GET /Account/Signup
    // Runs when someone visits the page. Renders Views/Account/Signup.cshtml
    // with an empty form.
    [HttpGet]
    public IActionResult Signup() => View();

    // POST /Account/Signup
    // Runs when the form is submitted. MVC automatically fills "model"
    // from the form fields (model binding) and runs the data annotation
    // checks ([Required], [StringLength], etc.), recording any failures in ModelState.
    [HttpPost]
    public IActionResult Signup(SignupModel model)
    {
        // Custom business rule that data annotations can't express.
        // Passing string.Empty as the key means the error isn't tied to any field,
        // so it shows up in asp-validation-summary="ModelOnly".
        // OrdinalIgnoreCase makes "admin", "Admin", and "ADMIN" all match.
        if (model.Username.Equals("admin", StringComparison.OrdinalIgnoreCase))
        {
            ModelState.AddModelError(string.Empty, "That username is reserved. Please choose another.");
        }

        // ModelState.IsValid is false if any annotation failed or we added an error above.
        // Re-render the form with the user's input so they can fix it.
        // The error messages are displayed by the validation tag helpers in the view.
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // Success: store a one-time message that survives the redirect
        // and is cleared after it's read (the view reads it from TempData["Success"]).
        TempData["Success"] = $"Welcome, {model.Username}!";

        // Redirect to the GET version of Signup (Post/Redirect/Get pattern).
        // This stops the browser from re-submitting the form if the user refreshes.
        // nameof(Signup) gives the string "Signup" and stays safe if the method is renamed.
        return RedirectToAction(nameof(Signup));
    }
}
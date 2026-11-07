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
    //action method 
    //GET requests are commonly used to retrieve or display information.
    [HttpGet]
    public IActionResult Signup() => View();


    // Runs when the form is submitted. MVC automatically fills "model"
    // from the form fields 
    //POST requests are commonly used to submit data to a server
    [HttpPost]
    public IActionResult Signup(SignupModel model)


    {
        //if input from the field is 'admin' then show a asp-validation-summary="ModelOnly"

        if (model.Username.Equals("admin", StringComparison.OrdinalIgnoreCase))//the model here is the SignupModel
        {
            ModelState.AddModelError(string.Empty, "That username is reserved. Please choose another.");
        }


        //ModelState is an object that keeps track of the data submitted by the user and whether that data passes validation.

        //validation rules are commonly defined inside the Model class using Data Annotation Attributes or also called validation attributes
        if (!ModelState.IsValid)//if a validation attribute does not comply
        {
            return View(model);//returns the model displaying the errors in the span tag
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
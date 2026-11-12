// Brings in MVC types: Controller, IActionResult, [HttpGet], [HttpPost], etc.
using Microsoft.AspNetCore.Mvc;
// Brings in our SignupModel class.
using ValidationDemo.Models;


namespace ValidationDemo.Controllers;

// Inheriting from Controller gives access to View(), ModelState, TempData, etc.
public class AccountController : Controller
{
    //action method 
    [HttpGet]
    public IActionResult Signup() => View();


    // Runs when the form is submitted. MVC automatically fills "model"
    // from the form fields 
    [HttpPost]
    public IActionResult Signup(SignupModel model)
    {

        //check for model error
        //if input from the form is 'admin' then show a asp-validation-summary="ModelOnly"
        if (model.Username.Equals("admin", StringComparison.OrdinalIgnoreCase))//the model here is the SignupModel
        {
            ModelState.AddModelError(string.Empty, "That username is reserved. Please choose another.");//add model error
        }

        //ModelState.IsValid checks all validation errors, not just validation attributes or model attributes.
        //ModelState is an object that keeps track of the data submitted by the user and whether that data passes validation.
        //some validation rules are commonly defined inside the Model class using Data Annotation Attributes or also called validation attributes
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
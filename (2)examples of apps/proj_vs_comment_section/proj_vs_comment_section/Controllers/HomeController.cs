using Microsoft.AspNetCore.Mvc;
using proj_vs_comment_section.Models;

namespace proj_vs_comment_section.Controllers
{
    public class HomeController : Controller
    {
        // Temporary storage for comments
        static List<Comment> comments = new List<Comment>();

        // Show page
        public IActionResult Index()
        {
            return View(comments); //passing List to the view
        }

        // Add new comment
        [HttpPost]
        public IActionResult AddComment(Comment comment) //the tag asp-action="AddComment" brings you here making the input the parameter object
        {   
            comments.Add(comment);

            return RedirectToAction("Index");
        }
    }
}
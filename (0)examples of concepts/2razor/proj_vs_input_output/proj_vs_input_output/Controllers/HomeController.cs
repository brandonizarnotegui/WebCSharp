using Microsoft.AspNetCore.Mvc;
using proj_vs_input_output.Models;

namespace proj_vs_input_output.Controllers
{
    public class HomeController : Controller
    {

        //page will be available at localhost:<my_random_port>/home/index
        //input: Enter your name[________][Submit]  output:Hello Brandon 👋
        public IActionResult Index()
        {
            return View();
        }

        // Handles the form submission (POST request)
        [HttpPost]
        public IActionResult Greet(Person person)
        {
            return View(person);
        }
    }
}
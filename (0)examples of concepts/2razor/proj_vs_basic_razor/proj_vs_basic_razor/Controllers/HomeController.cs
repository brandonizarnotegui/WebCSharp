using Microsoft.AspNetCore.Mvc;
using proj_vs_basic_razor.Models;
using System.Diagnostics;

namespace proj_vs_basic_razor.Controllers
{
    public class HomeController : Controller
    {

        //localhost:<port>
        //output:proj_vs_basic_razor Home Privacy Person Information Jane 28
        public IActionResult Index()  //look for view called index
        {
            var person = new Person  //create person object
            {
                Name = "Jane",
                Age = 28
            };

            return View(person); // passes data to the view
        }

    }
}

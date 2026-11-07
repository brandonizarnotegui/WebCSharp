//this assumes you know about objects,methods,controller 

using Microsoft.AspNetCore.Mvc;
using proj_vs_basic_razor.Models;
using System.Diagnostics;

namespace proj_vs_basic_razor.Controllers
{
    public class HomeController : Controller
    {

        //page will be available at localhost:<my_random_port>/home/index
        //output:proj_vs_basic_razor Home Privacy Person Information Jane 28
        public IActionResult Index()  //look for view called index
        {
            var person = new Person  //create person object using an object initializer
            {
                Name = "Jane",
                Age = 28
            };

            return View(person); // passes data to the view
        }

    }
}

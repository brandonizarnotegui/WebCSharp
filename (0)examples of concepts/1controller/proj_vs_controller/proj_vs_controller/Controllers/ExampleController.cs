
using Microsoft.AspNetCore.Mvc;

namespace proj_vs_controller.Controllers
{
    public class ExampleController : Controller
    {

        //page will be available at localhost:<my_random_port>/example/first
        public IActionResult First()
        {
            return View();
        }
    }
}
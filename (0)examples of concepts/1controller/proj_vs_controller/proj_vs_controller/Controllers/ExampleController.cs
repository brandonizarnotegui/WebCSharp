
using Microsoft.AspNetCore.Mvc;

namespace proj_vs_controller.Controllers
{

    //a controller handles requests from the user
    public class ExampleController : Controller
    {

        //page will be available at localhost:<my_random_port>/example/first
        //this action method handles the get request trigered when the user navigates to this URL.
        public IActionResult First()
        {
            return View();//tells the controller to return a Razor View (.cshtml file) as the HTTP response to the browser.
        }
    }
}
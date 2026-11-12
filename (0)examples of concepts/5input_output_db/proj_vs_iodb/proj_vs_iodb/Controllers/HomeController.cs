using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using proj_vs_iodb.Data;
using proj_vs_iodb.Models;
using System.Numerics;

namespace proj_vs_iodb.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }


        // in ASP.NET Core applications, asynchronous database operations are generally
        // recommended because they help the server handle other requests while waiting for database operations to complete.
        public IActionResult Index()
        {
            var messages = _context.Messages.ToList();

            return View(messages);
        }

        [HttpPost]
        public IActionResult SaveMessage(string text)
        {
            Message message = new Message();

            message.Text = text;

            _context.Messages.Add(message); //calling a method on a property of an object. here the field is an object of type DbSet

            _context.SaveChanges();

            //We use RedirectToAction("Index") in SaveMessage() because
            //its main responsibility is to save the message to the database, not display a page.
            return RedirectToAction("Index");
        }
    }
}
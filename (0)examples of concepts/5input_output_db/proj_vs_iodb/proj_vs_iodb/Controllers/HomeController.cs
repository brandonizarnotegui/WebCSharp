using Microsoft.AspNetCore.Mvc;
using proj_vs_iodb.Data;
using proj_vs_iodb.Models;

namespace proj_vs_iodb.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

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

            _context.Messages.Add(message);

            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
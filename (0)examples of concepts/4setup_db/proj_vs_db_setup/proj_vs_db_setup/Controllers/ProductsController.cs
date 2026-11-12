// Controllers/ProductsController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using proj_vs_db_setup.Data;

namespace proj_vs_db_setup.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;

        //dependency injection of the AppDbContext into the controller (creates AppDbContext object and assigns it to the _context field)
        public ProductsController(AppDbContext context)
        {
            _context = context; //we will do database operation with _context
        }

        //This method runs asynchronously(uses Task) and, when it finishes, it returns an IActionResult.
        //as there is an asynchronous operation you must return an Task<IActionResult> instead of IActionResult
        public async Task<IActionResult> Index()
        {
            //extension method ToListAsync() this means the method is defined outside the class
            var products = await _context.Products.ToListAsync();//access products table and executes a query(in this case returns a list of all products)
            return View(products);
        }
    }
}
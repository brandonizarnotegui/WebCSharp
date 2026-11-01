// Data/AppDbContext.cs
using Microsoft.EntityFrameworkCore;
using proj_vs_db_setup.Models;

namespace proj_vs_db_setup.Data
{
    // AppDbContext represents the application's database.
    public class AppDbContext : DbContext
    {

        

        // Constructor
        // ASP.NET Core passes the database configuration (options)
        // to the base DbContext class through dependency injection.
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // Add your DbSets (tables) here
        // Represents the Products table in the database.
        // Each Product object corresponds to one row in the table.
        public DbSet<Product> Products { get; set; }
    }
}
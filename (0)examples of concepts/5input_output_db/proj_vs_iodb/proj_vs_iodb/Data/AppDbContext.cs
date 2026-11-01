using Microsoft.EntityFrameworkCore;
using proj_vs_iodb.Models;

namespace proj_vs_iodb.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Message> Messages { get; set; }
    }
}
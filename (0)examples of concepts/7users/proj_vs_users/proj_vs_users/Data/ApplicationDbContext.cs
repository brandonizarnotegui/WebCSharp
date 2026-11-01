using Microsoft.EntityFrameworkCore;
using LoginApp.Models;

namespace LoginApp.Data
{
    // The database context class.
    // This class manages the connection to the database and provides
    // access to the application's tables.
    public class ApplicationDbContext : DbContext
    {
        // Constructor.
        // Receives the database configuration (connection string, provider, etc.)
        // and passes it to the base DbContext class.
        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {
        }

        // Represents the Users table in the database.
        // EF Core will map the User class to a table named "Users".
        public DbSet<User> Users { get; set; }

        // This method is called by Entity Framework Core when it builds
        // the database model. It is used to configure tables, relationships,
        // indexes, constraints, and other database settings.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Calls the default model configuration from the base DbContext class.
            base.OnModelCreating(modelBuilder);

            // Configure the User entity (table).
            modelBuilder.Entity<User>(entity =>
            {
                // Create a unique index on the Username column.
                // This prevents two users from having the same username
                // and also makes username searches faster.
                entity.HasIndex(u => u.Username).IsUnique();

                // Create a unique index on the Email column.
                // This prevents duplicate email addresses and
                // improves the performance of email lookups.
                entity.HasIndex(u => u.Email).IsUnique();
            });
        }
    }
}
// Program.cs
using Microsoft.EntityFrameworkCore;
using proj_vs_db_setup.Data;


// Creates a WebApplicationBuilder object.
// This object is used to configure services and application settings.
var builder = WebApplication.CreateBuilder(args);

// Register AppDbContext as a service using dependency injection.
// This tells ASP.NET Core how to create an AppDbContext whenever
// one is requested by a controller or another service.
builder.Services.AddDbContext<AppDbContext>(options =>
{
    // Read the database connection string from appsettings.json.
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Seed test data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();

    if (!db.Products.Any())
    {
        db.Products.AddRange(
            new proj_vs_db_setup.Models.Product { Name = "Apple", Price = 1.99m },
            new proj_vs_db_setup.Models.Product { Name = "Banana", Price = 0.99m },
            new proj_vs_db_setup.Models.Product { Name = "Cherry", Price = 3.49m }
        );
        db.SaveChanges();
    }
}


app.Run();
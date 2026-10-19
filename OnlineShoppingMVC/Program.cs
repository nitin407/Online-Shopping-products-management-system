// Create the application builder.
var builder = WebApplication.CreateBuilder(args);

// Add MVC support.
builder.Services.AddControllersWithViews();

// Add Session support for storing the selected category.
builder.Services.AddSession();

// Add MemoryCache for product details.
builder.Services.AddMemoryCache();

// Add Response Caching support.
builder.Services.AddResponseCaching();

var app = builder.Build();

// Enable static files and routing.
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Enable response caching and session.
app.UseResponseCaching();
app.UseSession();

// Set the default page.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Shop}/{action=Index}/{id?}");

app.Run();

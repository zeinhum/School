using SchoolResultSystem.Web.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation();

// ✅ Add distributed memory cache and session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // session lifetime
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


builder.Services.AddDbContext<SchoolDbContext>(options =>
    options.UseSqlite("Data Source=SchoolDatabase.db"));

builder.WebHost.UseUrls("http://0.0.0.0:5000");

// Add this line to register the accessor
builder.Services.AddHttpContextAccessor();

// Ensure session is also registered
builder.Services.AddSession();

var app = builder.Build();

// ✅ Use session before routing
app.UseSession();
app.UseRouting();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // UseStaticFiles should be called before UseRouting



using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SchoolDbContext>();
    // EnsureCreated should be used for development/prototyping only.
    // For production, use migrations.
    db.Database.EnsureCreated();
}

// Map area routes first. This is crucial for correct routing.
// The {area:exists} constraint ensures this route only matches URLs that belong to an existing area.
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

// Map the default route last as a fallback.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

public partial class Program { } // testing access
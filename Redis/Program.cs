// Program.cs
using Microsoft.AspNetCore.Session;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();


// bydefault allows to all u need to handle this in your code.explore-->httpoptions in rest apis
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

//for specific websites

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("FacebookOnly", policy =>
//    {
//        policy.WithOrigins(
//                "https://www.facebook.com",
//                "https://facebook.com",
//                "https://m.facebook.com",
//                "https://business.facebook.com",
//                "https://developers.facebook.com"
//            )
//            .AllowAnyMethod()
//            .AllowAnyHeader()
//            .AllowCredentials(); // Important for session cookies
//    });
//});

// Configure Redis for session storage
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379"; // Your Redis connection string
    options.InstanceName = "SessionDemo_";
});



// Configure session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession(); // Enable session middleware
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
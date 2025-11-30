🚀 Redis Session Demo – ASP.NET Core MVC
A simple and practical example of session management using Redis in ASP.NET Core.
This project demonstrates how to store user session data outside the application memory using Redis, making it ideal for scalable, multi-server architectures.
________________________________________
📌 Features
✔ Login form with model binding (no AJAX needed)
✔ Sessions stored in Redis memory, not in-memory on the server
✔ Demonstrates how ASP.NET Core Session Middleware works
✔ Shows both JSON session object + individual session keys
✔ Dashboard that reads session values directly from Redis
✔ Page to inspect raw Redis session data
✔ Clean and simple UI
________________________________________
🧠 Why Redis for Sessions?
Normally, ASP.NET Core stores session in server memory.
But in scalable apps (multiple servers, load balancing), you need a shared session store.
Redis solves this:
•	🟥 Stores all sessions in memory (super fast)
•	🔄 Shared across multiple application instances
•	❌ Not lost when an app restarts
•	⚡ Perfect for distributed caching and session storage
________________________________________
🔍 Understanding the Magic: How Sessions Work With Redis
ASP.NET Core uses session middleware.
Once Redis is configured, you DO NOT handle Redis manually.
✔ Your usual session code:
HttpContext.Session.SetString("UserName", "admin");
var name = HttpContext.Session.GetString("UserName");
❤️ Behind the scenes:
Your Code → Session Middleware → Redis Memory
Redis becomes the storage engine.
You read/write sessions normally—middleware silently takes care of everything.
________________________________________
💡 Model Binding Explained (Why Login Works With No AJAX)
The login form uses plain HTML:
<input name="Username" />
<input name="Password" />
ASP.NET Core automatically maps these input names to the ViewModel:
public IActionResult Login(LoginViewModel model)
No JavaScript.
No AJAX.
No manual parameter passing.
Just simple, clean model binding.
________________________________________
🛠 Technologies Used
•	ASP.NET Core MVC
•	Redis (StackExchange.Redis)
•	Session Middleware
•	JSON Serialization
•	C# 12
•	Minimal Bootstrap/CSS styling
________________________________________
📦 Project Structure
/Controllers
    AccountController.cs
/Models
    UserSession.cs
    LoginViewModel.cs
/Views
    /Account
        Login.cshtml
        Dashboard.cshtml
        CheckRedis.cshtml
Program.cs
________________________________________
⚙️ How It Works Internally
1️⃣ User logs in
Credentials are validated against an in-memory user list.
2️⃣ User session is created
var sessionJson = JsonSerializer.Serialize(userSession);
HttpContext.Session.SetString("UserSession", sessionJson);
3️⃣ Session is stored in Redis
Redis uses keys like:
SessionDemo_<SessionId>
4️⃣ Dashboard retrieves data from Redis
var sessionData = HttpContext.Session.GetString("UserSession");
5️⃣ Logging out clears the Redis session
HttpContext.Session.Clear();
________________________________________
🔧 Redis Configuration (Program.cs)
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
    options.InstanceName = "SessionDemo_";
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
This means:
•	Redis handles all session storage
•	You use session normally
•	Everything is stored/retrieved from Redis memory
________________________________________
🖥️ Screens Included
✔ Login Page
Simple form using MVC model binding.
✔ Dashboard
Shows session values retrieved from Redis.
✔ Redis Inspector
Displays:
•	Session ID
•	JSON session object
•	Individual values
Perfect for debugging.
________________________________________
▶️ Running the Project
1. Start Redis locally
If using Docker:
docker run -d -p 6379:6379 redis
2. Run the ASP.NET Core project
dotnet run
3. Navigate to:
https://localhost:<port>/Account/Login
________________________________________
📝 Test Credentials
Username	Password	Role
admin	admin123	Administrator
john	john123	User
sarah	sarah123	Premium User
________________________________________
🎯 What You Learn from This Project
•	How redis session middleware works
•	How session data is stored outside the application
•	How model binding works without AJAX
•	How to view raw Redis session data
•	How to serialize complex objects into session
________________________________________
⭐ Final Notes
This project is the perfect starting point for:
•	Scalable apps
•	Load balanced systems
•	Microservices
•	APIs needing shared session state
Redis makes session management fast, stateless, and reliable.


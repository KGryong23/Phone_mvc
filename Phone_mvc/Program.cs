using Microsoft.AspNetCore.Authentication.Cookies;
using Phone_mvc;
using Phone_mvc.Middlewares;
using Phone_mvc.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServices(builder.Configuration);

// Thêm dịch vụ
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/User/Login";              // Khi chưa login thì chuyển về đây
        options.LogoutPath = "/User/Logout";
        options.AccessDeniedPath = "/User/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);  // Thời hạn cookie
        options.SlidingExpiration = true;                   // Gia hạn nếu người dùng hoạt động
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    });

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddMemoryCache();

builder.Services.AddHostedService<PermissionSyncHostedService>();

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();

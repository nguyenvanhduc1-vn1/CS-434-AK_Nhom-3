using AspNetCoreHero.ToastNotification;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using WebDatMonAn.Models.Settings;
using WebDatMonAn.Repository;
using WebDatMonAn.Repository.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure services
builder.Services.AddControllersWithViews();

// Configure DbContext
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectDB")));

// Configure Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(30);
    options.Cookie.IsEssential = true;
});
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<EmailServices>();

// Configure ToastNotification
builder.Services.AddNotyf(config =>
{
    config.DurationInSeconds = 3;
    config.IsDismissable = true;
    config.Position = NotyfPosition.TopRight;
});

// Configure Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.LoginPath = "/TaiKhoan/Login";
        options.LogoutPath = "/TaiKhoan/Logout";
        options.AccessDeniedPath = "/TaiKhoan/AccessDenied";
    })
    .AddCookie("AdminScheme", options =>
    {
        options.LoginPath = "/Admin/TaiKhoan/Login";
        options.LogoutPath = "/Admin/TaiKhoan/Logout";
        options.AccessDeniedPath = "/Admin/TaiKhoan/Login";
    })
.AddCookie("ShipperScheme", options =>
{
    options.LoginPath = "/Shipper/DangNhap/Login";
    options.LogoutPath = "/Shipper/DangNhap/login";
    options.AccessDeniedPath = "/Shipper/DangNhap/Login";
});

// Configure Authorization Policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy =>
        policy.RequireClaim("Role", "Admin"));
    options.AddPolicy("EmployeePolicy", policy =>
        policy.RequireClaim("Role", "Employee"));
    options.AddPolicy("ShipperPolicy", policy =>
        policy.RequireClaim("Role", "Shipper")); ;
});

var app = builder.Build();

// Middleware configuration
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "Areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "Areas",
    pattern: "{area:exists}/{controller=QuanTri}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

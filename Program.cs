using Microsoft.EntityFrameworkCore;
using FinalExamProject.Models;

var builder = WebApplication.CreateBuilder(args);

// 1) Services
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<GoShopContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GoShopConnection")));

builder.Services.AddDistributedMemoryCache();   // 給 Session 用的快取
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// 給需要直接取用 HttpContext 的地方（例如 Controller 以外的類別）
builder.Services.AddHttpContextAccessor();

// 只是在這裡設定 Https 轉導的 Port（對 UseHttpsRedirection 的配置）
builder.Services.AddHttpsRedirection(options =>
{
    options.HttpsPort = 7143;   // 依你的 https 埠號
});

var app = builder.Build();

// 2) Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();        // Session 必須在 UseRouting 之後、MapControllerRoute 之前
app.UseAuthorization();

// 3) Routes
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// 4) Seed
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    SeedData.Initialize(services);
}

app.Run();

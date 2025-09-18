using Microsoft.EntityFrameworkCore;
using FinalExamProject.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<GoShopContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GoShopConnection")));

// ✅ 在這裡設定 HTTPS 轉導要用的埠（換成你的 https 埠號）
builder.Services.AddHttpsRedirection(options =>
{
    options.HttpsPort = 7143;   // ← 改成你的 https 埠
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// ✅ 這裡用無參數版本
app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    SeedData.Initialize(services);
}
app.UseHttpsRedirection();

app.Run();

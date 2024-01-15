using Microsoft.EntityFrameworkCore;
using QuanLySinhVien.Common;
using QuanLySinhVien.Models.Entity;

var builder = WebApplication.CreateBuilder(args);

// Add builder.Services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<QuanLySinhVienContext>(options =>
    options.UseSqlServer(DB.ConnectString));
//Server=PHONGLV\\SQLEXPRESS;Database=QuanLySinhVien;Trusted_Connection=True;
//Server=DEV-PHONGLV;Database=QuanLySinhVien;Trusted_Connection=True;

builder.Services.AddDistributedMemoryCache(); // Cần thêm dịch vụ bộ nhớ đệm cho phiên làm việc.
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian tồn tại của phiên (30 phút).
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

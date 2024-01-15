using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien.Models;
using QuanLySinhVien.Models.DTO;
using QuanLySinhVien.Models.Entity;
using System.Diagnostics;

namespace QuanLySinhVien.Controllers
{
    public class HomeController : Controller
    {

        private readonly QuanLySinhVienContext _context; 

        public HomeController(QuanLySinhVienContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(UserDto user)
        {
            if (ModelState.IsValid)
            {
                var userlogin = _context.Users.Where(c=>c.UserName == user.UserName && c.Password == user.Password).FirstOrDefault();
                if (userlogin == null)
                {
                    ModelState.AddModelError("", "username hoặc password không chính xác vui lòng thử lại");
                }
                else
                {
                    // Ghi dữ liệu vào phiên
                    HttpContext.Session.SetInt32("id", userlogin.Id);
                    HttpContext.Session.SetString("username", userlogin.UserName);
                    HttpContext.Session.SetInt32("role", userlogin.Role);
                    return RedirectToAction("Index","SinhVien");
                }
                
            }
            return View(user);
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult DoiMatKhau()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DoiMatKhau(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {

                var user = await _context.Users.Where(c=>c.Id == HttpContext.Session.GetInt32("id")).FirstOrDefaultAsync();
                if (user != null)
                {
                    user.Password = model.NewPassword;
                    _context.Users.Update(user);
                    _context.SaveChanges();

                    // Đặt thông báo vào TempData
                    TempData["SuccessMessage"] = "Thay đổi mật khẩu thành công";
                    return View(model);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Thay đổi mật khẩu thất bại");
                }
            }

            return View(model);
        }
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        public IActionResult Privacy()
        {
            return View();
        }
     

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
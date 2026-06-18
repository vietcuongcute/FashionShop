using FashionShop.Web.Data;
using FashionShop.Web.Models.Entities;
using FashionShop.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FashionShop.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly FashionShopDbContext _context;

        public AccountController(FashionShopDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (IsLoggedIn())
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (IsLoggedIn())
            {
                return RedirectToAction("Index", "Home");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var emailDaTonTai = await _context.NguoiDungs
                .AnyAsync(x => x.Email == model.Email);

            if (emailDaTonTai)
            {
                ModelState.AddModelError("Email", "Email này đã được sử dụng.");
                return View(model);
            }

            var nguoiDung = new NguoiDung
            {
                HoTen = model.HoTen,
                Email = model.Email,
                MatKhau = model.MatKhau,
                SoDienThoai = model.SoDienThoai,
                DiaChi = model.DiaChi,
                VaiTro = "Customer"
            };

            _context.NguoiDungs.Add(nguoiDung);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Đăng ký thành công. Vui lòng đăng nhập.";

            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (IsLoggedIn())
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (IsLoggedIn())
            {
                return RedirectToAction("Index", "Home");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var nguoiDung = await _context.NguoiDungs
                .FirstOrDefaultAsync(x => x.Email == model.Email && x.MatKhau == model.MatKhau);

            if (nguoiDung == null)
            {
                ModelState.AddModelError("", "Email hoặc mật khẩu không đúng.");
                return View(model);
            }

            HttpContext.Session.SetInt32("UserId", nguoiDung.Id);
            HttpContext.Session.SetString("UserName", nguoiDung.HoTen);
            HttpContext.Session.SetString("UserEmail", nguoiDung.Email);
            HttpContext.Session.SetString("UserRole", nguoiDung.VaiTro);

            if (nguoiDung.VaiTro == "Admin")
            {
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserId");
            HttpContext.Session.Remove("UserName");
            HttpContext.Session.Remove("UserEmail");
            HttpContext.Session.Remove("UserRole");

            return RedirectToAction("Index", "Home");
        }

        private bool IsLoggedIn()
        {
            return HttpContext.Session.GetInt32("UserId") != null;
        }
    }
}
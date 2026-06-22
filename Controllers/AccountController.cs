using FashionShop.Web.Data;
using FashionShop.Web.Models.Entities;
using FashionShop.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Identity;

namespace FashionShop.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly FashionShopDbContext _context;
        private readonly PasswordHasher<NguoiDung> _passwordHasher = new PasswordHasher<NguoiDung>();
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
                HoTen = model.HoTen.Trim(),
                Email = model.Email.Trim(),
                SoDienThoai = model.SoDienThoai,
                DiaChi = model.DiaChi,
                VaiTro = "Customer"
            };

            nguoiDung.MatKhau = _passwordHasher.HashPassword(nguoiDung, model.MatKhau);

            _context.NguoiDungs.Add(nguoiDung);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Đăng ký thành công. Vui lòng đăng nhập.";

            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (IsLoggedIn())
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            if (IsLoggedIn())
            {
                return RedirectToAction("Index", "Home");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.ReturnUrl = returnUrl;
                return View(model);
            }

            var email = model.Email.Trim();
            var nguoiDung = await _context.NguoiDungs
    .FirstOrDefaultAsync(x => x.Email == email);

            if (nguoiDung == null)
            {
                ModelState.AddModelError("", "Email hoặc mật khẩu không đúng.");
                ViewBag.ReturnUrl = returnUrl;
                return View(model);
            }

            var verifyResult = _passwordHasher.VerifyHashedPassword(
                nguoiDung,
                nguoiDung.MatKhau,
                model.MatKhau
            );

            if (verifyResult == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError("", "Email hoặc mật khẩu không đúng.");
                ViewBag.ReturnUrl = returnUrl;
                return View(model);
            }

            HttpContext.Session.SetInt32("UserId", nguoiDung.Id);
            HttpContext.Session.SetString("UserName", nguoiDung.HoTen);
            HttpContext.Session.SetString("UserEmail", nguoiDung.Email);
            HttpContext.Session.SetString("UserRole", nguoiDung.VaiTro);

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            if (nguoiDung.VaiTro == "Admin")
            {
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction(nameof(Login), new { returnUrl = Url.Action(nameof(Profile), "Account") });
            }

            var user = await _context.NguoiDungs.FindAsync(userId.Value);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(NguoiDung model)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction(nameof(Login));
            }

            var user = await _context.NguoiDungs.FindAsync(userId.Value);
            if (user == null)
            {
                return NotFound();
            }

            ModelState.Remove("MatKhau");
            ModelState.Remove("VaiTro");
            ModelState.Remove("DonHangs");

            if (string.IsNullOrWhiteSpace(model.HoTen))
            {
                ModelState.AddModelError("HoTen", "Họ tên không được để trống.");
            }

            if (string.IsNullOrWhiteSpace(model.Email))
            {
                ModelState.AddModelError("Email", "Email không được để trống.");
            }
            else
            {
                var emailTrung = await _context.NguoiDungs.AnyAsync(x => x.Id != user.Id && x.Email == model.Email);
                if (emailTrung)
                {
                    ModelState.AddModelError("Email", "Email này đã được sử dụng.");
                }
            }

            if (!ModelState.IsValid)
            {
                model.Id = user.Id;
                model.VaiTro = user.VaiTro;
                model.MatKhau = user.MatKhau;
                return View(model);
            }

            user.HoTen = model.HoTen.Trim();
            user.Email = model.Email.Trim();
            user.SoDienThoai = model.SoDienThoai;
            user.DiaChi = model.DiaChi;

            if (!string.IsNullOrWhiteSpace(model.MatKhau))
            {
                user.MatKhau = _passwordHasher.HashPassword(user, model.MatKhau);
            }

            await _context.SaveChangesAsync();

            HttpContext.Session.SetString("UserName", user.HoTen);
            HttpContext.Session.SetString("UserEmail", user.Email);
            TempData["Success"] = "Cập nhật hồ sơ thành công.";

            return RedirectToAction(nameof(Profile));
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

using System.Diagnostics;
using FashionShop.Web.Data;
using FashionShop.Web.Models;
using FashionShop.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FashionShop.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly FashionShopDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(FashionShopDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var model = new HomeViewModel
            {
                DanhMucs = await _context.DanhMucs
                    .Include(x => x.SanPhams)
                    .OrderBy(x => x.TenDanhMuc)
                    .ToListAsync(),

                SanPhamNoiBat = await _context.SanPhams
                    .Include(sp => sp.DanhMuc)
                    .Where(sp => sp.NoiBat)
                    .OrderByDescending(sp => sp.NgayTao)
                    .Take(8)
                    .ToListAsync(),

                SanPhamMoi = await _context.SanPhams
                    .Include(sp => sp.DanhMuc)
                    .OrderByDescending(sp => sp.NgayTao)
                    .Take(8)
                    .ToListAsync(),

                TongSanPham = await _context.SanPhams.CountAsync(),
                TongDanhMuc = await _context.DanhMucs.CountAsync()
            };

            return View(model);
        }

        public IActionResult About()
        {
            ViewData["Title"] = "Về VyAnhFashion";
            return View();
        }

        [HttpGet]
        public IActionResult Contact()
        {
            ViewData["Title"] = "Liên hệ";
            return View(new ContactViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Contact(ContactViewModel model)
        {
            ViewData["Title"] = "Liên hệ";

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            TempData["Success"] = "VyAnhFashion đã nhận thông tin liên hệ. Bộ phận chăm sóc khách hàng sẽ phản hồi sớm nhất.";
            ModelState.Clear();
            return View(new ContactViewModel());
        }

        public IActionResult Privacy()
        {
            return RedirectToAction(nameof(About));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}

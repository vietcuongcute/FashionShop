using System.Diagnostics;
using FashionShop.Web.Data;
using FashionShop.Web.Models;
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
            var sanPhams = await _context.SanPhams
                .Include(sp => sp.DanhMuc)
                .OrderByDescending(sp => sp.NgayTao)
                .Take(8)
                .ToListAsync();

            return View(sanPhams);
        }

        public IActionResult Privacy()
        {
            return View();
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
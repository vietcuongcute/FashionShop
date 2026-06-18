using FashionShop.Web.Data;
using FashionShop.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FashionShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        private readonly FashionShopDbContext _context;

        public DashboardController(FashionShopDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            var model = new AdminDashboardViewModel
            {
                TongSanPham = await _context.SanPhams.CountAsync(),
                TongDanhMuc = await _context.DanhMucs.CountAsync(),
                TongDonHang = await _context.DonHangs.CountAsync(),
                TongKhachHang = await _context.NguoiDungs.CountAsync(x => x.VaiTro == "Customer"),

                TongDoanhThu = await _context.DonHangs
                    .Where(x => x.TrangThai != "Đã hủy")
                    .SumAsync(x => x.TongTien),

                DonHangMoiNhat = await _context.DonHangs
                    .OrderByDescending(x => x.NgayDat)
                    .Take(5)
                    .ToListAsync(),

                SanPhamSapHetHang = await _context.SanPhams
                    .Include(x => x.DanhMuc)
                    .Where(x => x.SoLuongTon <= 10)
                    .OrderBy(x => x.SoLuongTon)
                    .Take(5)
                    .ToListAsync()
            };

            return View(model);
        }

        private bool IsAdmin()
        {
            var role = HttpContext.Session.GetString("UserRole");
            return role == "Admin";
        }
    }
}
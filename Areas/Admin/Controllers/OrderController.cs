using FashionShop.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FashionShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly FashionShopDbContext _context;

        public OrderController(FashionShopDbContext context)
        {
            _context = context;
        }

        private bool IsAdmin()
        {
            var role = HttpContext.Session.GetString("UserRole");
            return role == "Admin";
        }

        public async Task<IActionResult> Index(string? keyword, string? trangThai)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            var query = _context.DonHangs
                .Include(x => x.NguoiDung)
                .Include(x => x.ChiTietDonHangs)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(x =>
                    x.HoTenNguoiNhan.Contains(keyword) ||
                    x.SoDienThoai.Contains(keyword));
            }

            if (!string.IsNullOrWhiteSpace(trangThai))
            {
                query = query.Where(x => x.TrangThai == trangThai);
            }

            var donHangs = await query
                .OrderByDescending(x => x.NgayDat)
                .ToListAsync();

            ViewBag.Keyword = keyword;
            ViewBag.TrangThai = trangThai;

            ViewBag.TrangThaiList = new List<string>
            {
                "Chờ xác nhận",
                "Đã xác nhận",
                "Đang giao",
                "Hoàn thành",
                "Đã hủy"
            };

            return View(donHangs);
        }

        public async Task<IActionResult> Details(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            var donHang = await _context.DonHangs
                .Include(x => x.NguoiDung)
                .Include(x => x.ChiTietDonHangs)
                    .ThenInclude(x => x.SanPham)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (donHang == null)
            {
                return NotFound();
            }

            ViewBag.TrangThaiList = new List<string>
            {
                "Chờ xác nhận",
                "Đã xác nhận",
                "Đang giao",
                "Hoàn thành",
                "Đã hủy"
            };

            return View(donHang);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, string trangThai)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            var donHang = await _context.DonHangs.FindAsync(id);

            if (donHang == null)
            {
                return NotFound();
            }

            var trangThaiHopLe = new List<string>
            {
                "Chờ xác nhận",
                "Đã xác nhận",
                "Đang giao",
                "Hoàn thành",
                "Đã hủy"
            };

            if (!trangThaiHopLe.Contains(trangThai))
            {
                TempData["ErrorMessage"] = "Trạng thái đơn hàng không hợp lệ.";
                return RedirectToAction(nameof(Details), new { id });
            }

            donHang.TrangThai = trangThai;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Cập nhật trạng thái đơn hàng thành công.";
            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
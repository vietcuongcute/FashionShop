using FashionShop.Web.Data;
using FashionShop.Web.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FashionShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly FashionShopDbContext _context;

        public ProductController(FashionShopDbContext context)
        {
            _context = context;
        }

        private bool IsAdmin()
        {
            var role = HttpContext.Session.GetString("UserRole");
            return role == "Admin";
        }

        private async Task LoadDanhMucSelectList(int? selectedId = null)
        {
            ViewBag.DanhMucs = new SelectList(
                await _context.DanhMucs.OrderBy(x => x.TenDanhMuc).ToListAsync(),
                "Id",
                "TenDanhMuc",
                selectedId
            );
        }

        public async Task<IActionResult> Index(string? keyword, int? danhMucId)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            var query = _context.SanPhams
                .Include(x => x.DanhMuc)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(x => x.TenSanPham.Contains(keyword));
            }

            if (danhMucId.HasValue && danhMucId.Value > 0)
            {
                query = query.Where(x => x.DanhMucId == danhMucId.Value);
            }

            var sanPhams = await query
                .OrderByDescending(x => x.Id)
                .ToListAsync();

            ViewBag.Keyword = keyword;
            ViewBag.DanhMucId = danhMucId;

            ViewBag.DanhMucs = new SelectList(
                await _context.DanhMucs.OrderBy(x => x.TenDanhMuc).ToListAsync(),
                "Id",
                "TenDanhMuc",
                danhMucId
            );

            return View(sanPhams);
        }

        public async Task<IActionResult> Create()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            await LoadDanhMucSelectList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SanPham sanPham)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            if (!ModelState.IsValid)
            {
                await LoadDanhMucSelectList(sanPham.DanhMucId);
                return View(sanPham);
            }

            sanPham.NgayTao = DateTime.Now;

            _context.SanPhams.Add(sanPham);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Thêm sản phẩm thành công.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            var sanPham = await _context.SanPhams.FindAsync(id);

            if (sanPham == null)
            {
                return NotFound();
            }

            await LoadDanhMucSelectList(sanPham.DanhMucId);
            return View(sanPham);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SanPham sanPham)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            if (id != sanPham.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                await LoadDanhMucSelectList(sanPham.DanhMucId);
                return View(sanPham);
            }

            var sanPhamCanSua = await _context.SanPhams.FindAsync(id);

            if (sanPhamCanSua == null)
            {
                return NotFound();
            }

            sanPhamCanSua.TenSanPham = sanPham.TenSanPham;
            sanPhamCanSua.Gia = sanPham.Gia;
            sanPhamCanSua.SoLuongTon = sanPham.SoLuongTon;
            sanPhamCanSua.MoTa = sanPham.MoTa;
            sanPhamCanSua.HinhAnh = sanPham.HinhAnh;
            sanPhamCanSua.NoiBat = sanPham.NoiBat;
            sanPhamCanSua.DanhMucId = sanPham.DanhMucId;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Cập nhật sản phẩm thành công.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            var sanPham = await _context.SanPhams
                .Include(x => x.DanhMuc)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (sanPham == null)
            {
                return NotFound();
            }

            return View(sanPham);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            var sanPham = await _context.SanPhams.FindAsync(id);

            if (sanPham == null)
            {
                return NotFound();
            }

            _context.SanPhams.Remove(sanPham);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Xóa sản phẩm thành công.";
            return RedirectToAction(nameof(Index));
        }
    }
}
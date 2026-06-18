using FashionShop.Web.Data;
using FashionShop.Web.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FashionShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly FashionShopDbContext _context;

        public CategoryController(FashionShopDbContext context)
        {
            _context = context;
        }

        private bool IsAdmin()
        {
            var role = HttpContext.Session.GetString("UserRole");
            return role == "Admin";
        }

        public async Task<IActionResult> Index()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            var danhMucs = await _context.DanhMucs
                .Include(x => x.SanPhams)
                .OrderByDescending(x => x.Id)
                .ToListAsync();

            return View(danhMucs);
        }

        public IActionResult Create()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DanhMuc danhMuc)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            if (!ModelState.IsValid)
            {
                return View(danhMuc);
            }

            var tenBiTrung = await _context.DanhMucs
                .AnyAsync(x => x.TenDanhMuc.ToLower() == danhMuc.TenDanhMuc.ToLower());

            if (tenBiTrung)
            {
                ModelState.AddModelError("TenDanhMuc", "Tên danh mục này đã tồn tại.");
                return View(danhMuc);
            }

            _context.DanhMucs.Add(danhMuc);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Thêm danh mục thành công.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            var danhMuc = await _context.DanhMucs.FindAsync(id);

            if (danhMuc == null)
            {
                return NotFound();
            }

            return View(danhMuc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DanhMuc danhMuc)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            if (id != danhMuc.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(danhMuc);
            }

            var tenBiTrung = await _context.DanhMucs
                .AnyAsync(x => x.Id != danhMuc.Id &&
                               x.TenDanhMuc.ToLower() == danhMuc.TenDanhMuc.ToLower());

            if (tenBiTrung)
            {
                ModelState.AddModelError("TenDanhMuc", "Tên danh mục này đã tồn tại.");
                return View(danhMuc);
            }

            var danhMucCanSua = await _context.DanhMucs.FindAsync(id);

            if (danhMucCanSua == null)
            {
                return NotFound();
            }

            danhMucCanSua.TenDanhMuc = danhMuc.TenDanhMuc;
            danhMucCanSua.MoTa = danhMuc.MoTa;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Cập nhật danh mục thành công.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            var danhMuc = await _context.DanhMucs
                .Include(x => x.SanPhams)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (danhMuc == null)
            {
                return NotFound();
            }

            return View(danhMuc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            var danhMuc = await _context.DanhMucs
                .Include(x => x.SanPhams)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (danhMuc == null)
            {
                return NotFound();
            }

            if (danhMuc.SanPhams.Any())
            {
                TempData["ErrorMessage"] = "Không thể xóa danh mục vì đang có sản phẩm thuộc danh mục này.";
                return RedirectToAction(nameof(Index));
            }

            _context.DanhMucs.Remove(danhMuc);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Xóa danh mục thành công.";
            return RedirectToAction(nameof(Index));
        }
    }
}
using FashionShop.Web.Data;
using FashionShop.Web.Filters;
using FashionShop.Web.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FashionShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AdminAuthorize]
    public class CustomerController : Controller
    {
        private readonly FashionShopDbContext _context;

        public CustomerController(FashionShopDbContext context)
        {
            _context = context;
        }

        private bool IsAdmin()
        {
            var role = HttpContext.Session.GetString("UserRole");
            return role == "Admin";
        }

        public async Task<IActionResult> Index(string? keyword)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            var query = _context.NguoiDungs
                .Include(x => x.DonHangs)
                .Where(x => x.VaiTro == "Customer")
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(x =>
                    x.HoTen.Contains(keyword) ||
                    x.Email.Contains(keyword) ||
                    (x.SoDienThoai != null && x.SoDienThoai.Contains(keyword)));
            }

            var customers = await query
                .OrderByDescending(x => x.Id)
                .ToListAsync();

            ViewBag.Keyword = keyword;

            return View(customers);
        }

        public async Task<IActionResult> Details(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            var customer = await _context.NguoiDungs
                .Include(x => x.DonHangs)
                .ThenInclude(x => x.ChiTietDonHangs)
                .FirstOrDefaultAsync(x => x.Id == id && x.VaiTro == "Customer");

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            var customer = await _context.NguoiDungs
                .FirstOrDefaultAsync(x => x.Id == id && x.VaiTro == "Customer");

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NguoiDung model)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            if (id != model.Id)
            {
                return BadRequest();
            }

            var customer = await _context.NguoiDungs
                .FirstOrDefaultAsync(x => x.Id == id && x.VaiTro == "Customer");

            if (customer == null)
            {
                return NotFound();
            }

            ModelState.Remove("MatKhau");
            ModelState.Remove("VaiTro");
            ModelState.Remove("DonHangs");

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var emailBiTrung = await _context.NguoiDungs
                .AnyAsync(x => x.Id != id && x.Email == model.Email);

            if (emailBiTrung)
            {
                ModelState.AddModelError("Email", "Email này đã được sử dụng.");
                return View(model);
            }

            customer.HoTen = model.HoTen;
            customer.Email = model.Email;
            customer.SoDienThoai = model.SoDienThoai;
            customer.DiaChi = model.DiaChi;

            if (!string.IsNullOrWhiteSpace(model.MatKhau))
            {
                customer.MatKhau = model.MatKhau;
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Cập nhật khách hàng thành công.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            var customer = await _context.NguoiDungs
                .Include(x => x.DonHangs)
                .FirstOrDefaultAsync(x => x.Id == id && x.VaiTro == "Customer");

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            var customer = await _context.NguoiDungs
                .Include(x => x.DonHangs)
                .FirstOrDefaultAsync(x => x.Id == id && x.VaiTro == "Customer");

            if (customer == null)
            {
                return NotFound();
            }

            if (customer.DonHangs.Any())
            {
                TempData["ErrorMessage"] = "Không thể xóa khách hàng vì khách hàng này đã có đơn hàng.";
                return RedirectToAction(nameof(Index));
            }

            _context.NguoiDungs.Remove(customer);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Xóa khách hàng thành công.";
            return RedirectToAction(nameof(Index));
        }
    }
}
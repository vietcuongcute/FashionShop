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
        private readonly IWebHostEnvironment _environment;

        public ProductController(FashionShopDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
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

        private async Task<string?> SaveImageAsync(IFormFile? file)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
            {
                throw new InvalidOperationException("Chỉ cho phép upload ảnh .jpg, .jpeg, .png, .webp");
            }

            var folderPath = Path.Combine(_environment.WebRootPath, "images", "products");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(folderPath, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/images/products/{fileName}";
        }

        public async Task<IActionResult> Index(string? keyword, int? danhMucId)
        {

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

            await LoadDanhMucSelectList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SanPham sanPham, IFormFile? imageFile)
        {

            if (!ModelState.IsValid)
            {
                await LoadDanhMucSelectList(sanPham.DanhMucId);
                return View(sanPham);
            }

            try
            {
                var imageUrl = await SaveImageAsync(imageFile);

                if (!string.IsNullOrWhiteSpace(imageUrl))
                {
                    sanPham.HinhAnh = imageUrl;
                }
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("HinhAnh", ex.Message);
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
        public async Task<IActionResult> Edit(int id, SanPham sanPham, IFormFile? imageFile)
        {
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
            sanPhamCanSua.NoiBat = sanPham.NoiBat;
            sanPhamCanSua.DanhMucId = sanPham.DanhMucId;

            if (imageFile != null && imageFile.Length > 0)
            {
                try
                {
                    var imageUrl = await SaveImageAsync(imageFile);

                    if (!string.IsNullOrWhiteSpace(imageUrl))
                    {
                        sanPhamCanSua.HinhAnh = imageUrl;
                    }
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError("HinhAnh", ex.Message);
                    sanPham.HinhAnh = sanPhamCanSua.HinhAnh;
                    await LoadDanhMucSelectList(sanPham.DanhMucId);
                    return View(sanPham);
                }
            }
            else
            {
                sanPhamCanSua.HinhAnh = sanPham.HinhAnh;
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Cập nhật sản phẩm thành công.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
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
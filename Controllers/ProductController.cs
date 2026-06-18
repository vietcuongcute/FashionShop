using FashionShop.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FashionShop.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly FashionShopDbContext _context;

        public ProductController(FashionShopDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? categoryId, string? keyword, string? sortOrder)
        {
            var sanPhamQuery = _context.SanPhams
                .Include(sp => sp.DanhMuc)
                .AsQueryable();

            if (categoryId.HasValue)
            {
                sanPhamQuery = sanPhamQuery.Where(sp => sp.DanhMucId == categoryId.Value);
            }

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                sanPhamQuery = sanPhamQuery.Where(sp =>
                    sp.TenSanPham.Contains(keyword) ||
                    (sp.MoTa != null && sp.MoTa.Contains(keyword)));
            }

            sanPhamQuery = sortOrder switch
            {
                "price_asc" => sanPhamQuery.OrderBy(sp => sp.Gia),
                "price_desc" => sanPhamQuery.OrderByDescending(sp => sp.Gia),
                "newest" => sanPhamQuery.OrderByDescending(sp => sp.NgayTao),
                _ => sanPhamQuery.OrderByDescending(sp => sp.NgayTao)
            };

            ViewBag.DanhMucs = await _context.DanhMucs.ToListAsync();
            ViewBag.CategoryId = categoryId;
            ViewBag.Keyword = keyword;
            ViewBag.SortOrder = sortOrder;

            var sanPhams = await sanPhamQuery.ToListAsync();

            return View(sanPhams);
        }

        public async Task<IActionResult> Details(int id)
        {
            var sanPham = await _context.SanPhams
                .Include(sp => sp.DanhMuc)
                .FirstOrDefaultAsync(sp => sp.Id == id);

            if (sanPham == null)
            {
                return NotFound();
            }

            var sanPhamLienQuan = await _context.SanPhams
                .Include(sp => sp.DanhMuc)
                .Where(sp => sp.DanhMucId == sanPham.DanhMucId && sp.Id != sanPham.Id)
                .Take(4)
                .ToListAsync();

            ViewBag.SanPhamLienQuan = sanPhamLienQuan;

            return View(sanPham);
        }
    }
}
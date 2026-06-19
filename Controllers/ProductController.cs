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

        public async Task<IActionResult> Index(int? categoryId, string? keyword, string? sortOrder, decimal? minPrice, decimal? maxPrice, bool? inStock)
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
                keyword = keyword.Trim();
                sanPhamQuery = sanPhamQuery.Where(sp =>
                    sp.TenSanPham.Contains(keyword) ||
                    (sp.MoTa != null && sp.MoTa.Contains(keyword)) ||
                    (sp.DanhMuc != null && sp.DanhMuc.TenDanhMuc.Contains(keyword)));
            }

            if (minPrice.HasValue)
            {
                sanPhamQuery = sanPhamQuery.Where(sp => sp.Gia >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                sanPhamQuery = sanPhamQuery.Where(sp => sp.Gia <= maxPrice.Value);
            }

            if (inStock == true)
            {
                sanPhamQuery = sanPhamQuery.Where(sp => sp.SoLuongTon > 0);
            }

            sanPhamQuery = sortOrder switch
            {
                "price_asc" => sanPhamQuery.OrderBy(sp => sp.Gia),
                "price_desc" => sanPhamQuery.OrderByDescending(sp => sp.Gia),
                "name_asc" => sanPhamQuery.OrderBy(sp => sp.TenSanPham),
                "featured" => sanPhamQuery.OrderByDescending(sp => sp.NoiBat).ThenByDescending(sp => sp.NgayTao),
                "newest" => sanPhamQuery.OrderByDescending(sp => sp.NgayTao),
                _ => sanPhamQuery.OrderByDescending(sp => sp.NgayTao)
            };

            ViewBag.DanhMucs = await _context.DanhMucs.OrderBy(x => x.TenDanhMuc).ToListAsync();
            ViewBag.CategoryId = categoryId;
            ViewBag.Keyword = keyword;
            ViewBag.SortOrder = sortOrder;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;
            ViewBag.InStock = inStock;

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
                .OrderByDescending(sp => sp.NoiBat)
                .ThenByDescending(sp => sp.NgayTao)
                .Take(4)
                .ToListAsync();

            ViewBag.SanPhamLienQuan = sanPhamLienQuan;

            return View(sanPham);
        }
    }
}

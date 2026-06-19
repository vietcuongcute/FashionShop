using FashionShop.Web.Data;
using FashionShop.Web.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FashionShop.Web.Controllers
{
    public class WishlistController : Controller
    {
        private const string WishlistSessionKey = "FashionShopWishlist";
        private readonly FashionShopDbContext _context;

        public WishlistController(FashionShopDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("UserRole") == "Admin")
            {
                TempData["Error"] = "Admin không xem danh sách yêu thích của khách hàng. Wishlist là dữ liệu riêng của người mua.";
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }

            var ids = GetWishlist();
            var products = await _context.SanPhams
                .Include(x => x.DanhMuc)
                .Where(x => ids.Contains(x.Id))
                .OrderByDescending(x => x.NgayTao)
                .ToListAsync();

            return View(products);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Toggle(int sanPhamId, string? returnUrl = null)
        {
            if (HttpContext.Session.GetString("UserRole") == "Admin")
            {
                TempData["Error"] = "Tài khoản admin không sử dụng chức năng yêu thích của khách hàng.";
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }

            var exists = await _context.SanPhams.AnyAsync(x => x.Id == sanPhamId);
            if (!exists)
            {
                return NotFound();
            }

            var ids = GetWishlist();
            if (ids.Contains(sanPhamId))
            {
                ids.Remove(sanPhamId);
                TempData["Success"] = "Đã xóa sản phẩm khỏi danh sách yêu thích.";
            }
            else
            {
                ids.Add(sanPhamId);
                TempData["Success"] = "Đã thêm sản phẩm vào danh sách yêu thích.";
            }

            SaveWishlist(ids);

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Remove(int sanPhamId)
        {
            if (HttpContext.Session.GetString("UserRole") == "Admin")
            {
                TempData["Error"] = "Tài khoản admin không sử dụng chức năng yêu thích của khách hàng.";
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }

            var ids = GetWishlist();
            ids.Remove(sanPhamId);
            SaveWishlist(ids);
            TempData["Success"] = "Đã xóa sản phẩm khỏi danh sách yêu thích.";
            return RedirectToAction(nameof(Index));
        }

        private List<int> GetWishlist()
        {
            return HttpContext.Session.GetObject<List<int>>(WishlistSessionKey) ?? new List<int>();
        }

        private void SaveWishlist(List<int> ids)
        {
            HttpContext.Session.SetObject(WishlistSessionKey, ids.Distinct().ToList());
        }
    }
}

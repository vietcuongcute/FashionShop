using FashionShop.Web.Data;
using FashionShop.Web.Extensions;
using FashionShop.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FashionShop.Web.Controllers
{
    public class CartController : Controller
    {
        private const string CartSessionKey = "FashionShopCart";

        private readonly FashionShopDbContext _context;

        public CartController(FashionShopDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var cart = GetCart();
            return View(cart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(int sanPhamId, int soLuong = 1, string? returnUrl = null)
        {
            if (soLuong <= 0)
            {
                soLuong = 1;
            }

            var sanPham = await _context.SanPhams
                .FirstOrDefaultAsync(sp => sp.Id == sanPhamId);

            if (sanPham == null)
            {
                return NotFound();
            }

            if (sanPham.SoLuongTon <= 0)
            {
                TempData["Error"] = "Sản phẩm đã hết hàng.";
                return RedirectToAction("Details", "Product", new { id = sanPhamId });
            }

            var cart = GetCart();

            var existingItem = cart.FirstOrDefault(x => x.SanPhamId == sanPhamId);

            if (existingItem == null)
            {
                cart.Add(new CartItemViewModel
                {
                    SanPhamId = sanPham.Id,
                    TenSanPham = sanPham.TenSanPham,
                    HinhAnh = sanPham.HinhAnh,
                    Gia = sanPham.Gia,
                    SoLuong = Math.Min(soLuong, sanPham.SoLuongTon),
                    SoLuongTon = sanPham.SoLuongTon
                });
            }
            else
            {
                var newQuantity = existingItem.SoLuong + soLuong;
                existingItem.SoLuong = Math.Min(newQuantity, sanPham.SoLuongTon);
                existingItem.SoLuongTon = sanPham.SoLuongTon;
            }

            SaveCart(cart);

            TempData["Success"] = "Đã thêm sản phẩm vào giỏ hàng.";

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateQuantity(int sanPhamId, int soLuong)
        {
            var cart = GetCart();

            var item = cart.FirstOrDefault(x => x.SanPhamId == sanPhamId);

            if (item == null)
            {
                return RedirectToAction("Index");
            }

            var sanPham = await _context.SanPhams
                .FirstOrDefaultAsync(sp => sp.Id == sanPhamId);

            if (sanPham == null)
            {
                cart.Remove(item);
                SaveCart(cart);
                return RedirectToAction("Index");
            }

            if (soLuong <= 0)
            {
                cart.Remove(item);
            }
            else
            {
                item.SoLuongTon = sanPham.SoLuongTon;
                item.SoLuong = Math.Min(soLuong, sanPham.SoLuongTon);
            }

            SaveCart(cart);

            TempData["Success"] = "Đã cập nhật giỏ hàng.";

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Remove(int sanPhamId)
        {
            var cart = GetCart();

            var item = cart.FirstOrDefault(x => x.SanPhamId == sanPhamId);

            if (item != null)
            {
                cart.Remove(item);
                SaveCart(cart);
                TempData["Success"] = "Đã xóa sản phẩm khỏi giỏ hàng.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Clear()
        {
            HttpContext.Session.Remove(CartSessionKey);
            TempData["Success"] = "Đã xóa toàn bộ giỏ hàng.";
            return RedirectToAction("Index");
        }

        private List<CartItemViewModel> GetCart()
        {
            var cart = HttpContext.Session.GetObject<List<CartItemViewModel>>(CartSessionKey);

            if (cart == null)
            {
                cart = new List<CartItemViewModel>();
            }

            return cart;
        }

        private void SaveCart(List<CartItemViewModel> cart)
        {
            HttpContext.Session.SetObject(CartSessionKey, cart);
        }
    }
}
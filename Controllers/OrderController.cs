using FashionShop.Web.Data;
using FashionShop.Web.Extensions;
using FashionShop.Web.Models.Entities;
using FashionShop.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FashionShop.Web.Controllers
{
    public class OrderController : Controller
    {
        private const string CartSessionKey = "FashionShopCart";

        private readonly FashionShopDbContext _context;

        public OrderController(FashionShopDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Checkout()
        {
            var cart = GetCart();

            if (!cart.Any())
            {
                TempData["Error"] = "Giỏ hàng đang trống, không thể đặt hàng.";
                return RedirectToAction("Index", "Cart");
            }

            var model = new CheckoutViewModel
            {
                CartItems = cart
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(CheckoutViewModel model)
        {
            var cart = GetCart();

            if (!cart.Any())
            {
                TempData["Error"] = "Giỏ hàng đang trống, không thể đặt hàng.";
                return RedirectToAction("Index", "Cart");
            }

            model.CartItems = cart;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                decimal tongTien = 0;

                foreach (var item in cart)
                {
                    var sanPham = await _context.SanPhams
                        .FirstOrDefaultAsync(sp => sp.Id == item.SanPhamId);

                    if (sanPham == null)
                    {
                        ModelState.AddModelError("", $"Sản phẩm {item.TenSanPham} không còn tồn tại.");
                        return View(model);
                    }

                    if (sanPham.SoLuongTon < item.SoLuong)
                    {
                        ModelState.AddModelError("", $"Sản phẩm {item.TenSanPham} chỉ còn {sanPham.SoLuongTon} sản phẩm.");
                        return View(model);
                    }

                    tongTien += sanPham.Gia * item.SoLuong;
                }

                var donHang = new DonHang
                {
                    NgayDat = DateTime.Now,
                    TongTien = tongTien,
                    TrangThai = "Chờ xác nhận",
                    HoTenNguoiNhan = model.HoTenNguoiNhan,
                    SoDienThoai = model.SoDienThoai,
                    DiaChiGiaoHang = model.DiaChiGiaoHang,
                    GhiChu = model.GhiChu
                };

                _context.DonHangs.Add(donHang);
                await _context.SaveChangesAsync();

                foreach (var item in cart)
                {
                    var sanPham = await _context.SanPhams
                        .FirstAsync(sp => sp.Id == item.SanPhamId);

                    var chiTiet = new ChiTietDonHang
                    {
                        DonHangId = donHang.Id,
                        SanPhamId = sanPham.Id,
                        SoLuong = item.SoLuong,
                        DonGia = sanPham.Gia,
                        ThanhTien = sanPham.Gia * item.SoLuong
                    };

                    _context.ChiTietDonHangs.Add(chiTiet);

                    sanPham.SoLuongTon -= item.SoLuong;
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                HttpContext.Session.Remove(CartSessionKey);

                return RedirectToAction("Success", new { id = donHang.Id });
            }
            catch
            {
                await transaction.RollbackAsync();

                ModelState.AddModelError("", "Có lỗi xảy ra khi đặt hàng. Vui lòng thử lại.");
                return View(model);
            }
        }

        public async Task<IActionResult> Success(int id)
        {
            var donHang = await _context.DonHangs
                .Include(dh => dh.ChiTietDonHangs)
                .ThenInclude(ct => ct.SanPham)
                .FirstOrDefaultAsync(dh => dh.Id == id);

            if (donHang == null)
            {
                return NotFound();
            }

            return View(donHang);
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
    }
}
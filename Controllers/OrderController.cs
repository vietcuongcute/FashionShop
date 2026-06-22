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
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var cart = GetCart();

            if (!cart.Any())
            {
                TempData["Error"] = "Giỏ hàng đang trống, không thể đặt hàng.";
                return RedirectToAction("Index", "Cart");
            }

            var currentUser = userId.HasValue ? _context.NguoiDungs.FirstOrDefault(x => x.Id == userId.Value) : null;

            var model = new CheckoutViewModel
            {
                CartItems = cart,
                HoTenNguoiNhan = currentUser?.HoTen ?? string.Empty,
                SoDienThoai = currentUser?.SoDienThoai ?? string.Empty,
                DiaChiGiaoHang = currentUser?.DiaChi ?? string.Empty,
                PhuongThucThanhToan = "COD"
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(CheckoutViewModel model)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

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
                        await transaction.RollbackAsync();
                        return View(model);
                    }

                    if (sanPham.SoLuongTon < item.SoLuong)
                    {
                        ModelState.AddModelError("", $"Sản phẩm {item.TenSanPham} chỉ còn {sanPham.SoLuongTon} sản phẩm.");
                        await transaction.RollbackAsync();
                        return View(model);
                    }

                    tongTien += sanPham.Gia * item.SoLuong;
                }

                var donHang = new DonHang
                {
                    NguoiDungId = userId.Value,
                    NgayDat = DateTime.Now,
                    TongTien = tongTien,
                    TrangThai = "Chờ xác nhận",
                    HoTenNguoiNhan = model.HoTenNguoiNhan,
                    SoDienThoai = model.SoDienThoai,
                    DiaChiGiaoHang = model.DiaChiGiaoHang,
                    GhiChu = BuildOrderNote(model.GhiChu, model.PhuongThucThanhToan)
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
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var donHang = await _context.DonHangs
                .Include(dh => dh.ChiTietDonHangs)
                .ThenInclude(ct => ct.SanPham)
                .FirstOrDefaultAsync(dh => dh.Id == id && dh.NguoiDungId == userId.Value);

            if (donHang == null)
            {
                return NotFound();
            }

            return View(donHang);
        }
        public async Task<IActionResult> History()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var donHangs = await _context.DonHangs
                .Where(x => x.NguoiDungId == userId.Value)
                .Include(x => x.ChiTietDonHangs)
                .OrderByDescending(x => x.NgayDat)
                .ToListAsync();

            return View(donHangs);
        }

        public async Task<IActionResult> Details(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var donHang = await _context.DonHangs
                .Include(x => x.ChiTietDonHangs)
                    .ThenInclude(x => x.SanPham)
                .FirstOrDefaultAsync(x => x.Id == id && x.NguoiDungId == userId.Value);

            if (donHang == null)
            {
                return NotFound();
            }

            return View(donHang);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var donHang = await _context.DonHangs
                .Include(x => x.ChiTietDonHangs)
                .ThenInclude(x => x.SanPham)
                .FirstOrDefaultAsync(x => x.Id == id && x.NguoiDungId == userId.Value);

            if (donHang == null)
            {
                return NotFound();
            }

            if (donHang.TrangThai != "Chờ xác nhận")
            {
                TempData["ErrorMessage"] = "Chỉ có thể hủy đơn hàng khi đơn còn ở trạng thái Chờ xác nhận.";
                return RedirectToAction(nameof(Details), new { id = donHang.Id });
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                donHang.TrangThai = "Đã hủy";

                foreach (var item in donHang.ChiTietDonHangs)
                {
                    if (item.SanPham != null)
                    {
                        item.SanPham.SoLuongTon += item.SoLuong;
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                TempData["SuccessMessage"] = "Hủy đơn hàng thành công.";
                return RedirectToAction(nameof(Details), new { id = donHang.Id });
            }
            catch
            {
                await transaction.RollbackAsync();

                TempData["ErrorMessage"] = "Có lỗi xảy ra khi hủy đơn hàng. Vui lòng thử lại.";
                return RedirectToAction(nameof(Details), new { id = donHang.Id });
            }
        }
        private static string? BuildOrderNote(string? ghiChu, string? phuongThucThanhToan)
        {
            var paymentText = phuongThucThanhToan switch
            {
                "Bank" => "Chuyển khoản ngân hàng",
                "Momo" => "Ví điện tử MoMo",
                _ => "Thanh toán khi nhận hàng"
            };

            if (string.IsNullOrWhiteSpace(ghiChu))
            {
                return $"Phương thức thanh toán: {paymentText}";
            }

            return $"{ghiChu.Trim()} | Phương thức thanh toán: {paymentText}";
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
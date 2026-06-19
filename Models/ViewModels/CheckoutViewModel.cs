using System.ComponentModel.DataAnnotations;

namespace FashionShop.Web.Models.ViewModels
{
    public class CheckoutViewModel
    {
        [Required(ErrorMessage = "Họ tên người nhận không được để trống")]
        [StringLength(100)]
        public string HoTenNguoiNhan { get; set; } = string.Empty;

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [StringLength(20)]
        public string SoDienThoai { get; set; } = string.Empty;

        [Required(ErrorMessage = "Địa chỉ giao hàng không được để trống")]
        public string DiaChiGiaoHang { get; set; } = string.Empty;

        public string? GhiChu { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn phương thức thanh toán")]
        public string PhuongThucThanhToan { get; set; } = "COD";

        public List<CartItemViewModel> CartItems { get; set; } = new List<CartItemViewModel>();

        public decimal TongTien => CartItems.Sum(x => x.ThanhTien);
    }
}
using System.ComponentModel.DataAnnotations;

namespace FashionShop.Web.Models.Entities
{
    public class NguoiDung
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Họ tên không được để trống")]
        [StringLength(100)]
        public string HoTen { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        public string MatKhau { get; set; } = string.Empty;

        [StringLength(20)]
        public string? SoDienThoai { get; set; }

        public string? DiaChi { get; set; }

        public string VaiTro { get; set; } = "Customer";

        public ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();
    }
}
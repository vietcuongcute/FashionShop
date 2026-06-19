using System.ComponentModel.DataAnnotations;

namespace FashionShop.Web.Models.ViewModels
{
    public class ContactViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        [StringLength(100)]
        public string HoTen { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = string.Empty;

        [StringLength(20)]
        public string? SoDienThoai { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập nội dung cần hỗ trợ")]
        [StringLength(1000, ErrorMessage = "Nội dung tối đa 1000 ký tự")]
        public string NoiDung { get; set; } = string.Empty;
    }
}

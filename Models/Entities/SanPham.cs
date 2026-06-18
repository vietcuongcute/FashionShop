using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FashionShop.Web.Models.Entities
{
    public class SanPham
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [StringLength(200)]
        public string TenSanPham { get; set; } = string.Empty;

        [Required(ErrorMessage = "Giá sản phẩm không được để trống")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá sản phẩm phải lớn hơn hoặc bằng 0")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Gia { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Số lượng tồn không hợp lệ")]
        public int SoLuongTon { get; set; }

        public string? MoTa { get; set; }

        public string? HinhAnh { get; set; }

        public bool NoiBat { get; set; }

        public DateTime NgayTao { get; set; } = DateTime.Now;

        public int DanhMucId { get; set; }

        public DanhMuc? DanhMuc { get; set; }
    }
}
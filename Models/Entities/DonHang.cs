using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FashionShop.Web.Models.Entities
{
    public class DonHang
    {
        public int Id { get; set; }

        public DateTime NgayDat { get; set; } = DateTime.Now;

        [Column(TypeName = "decimal(18,2)")]
        public decimal TongTien { get; set; }

        public string TrangThai { get; set; } = "Chờ xác nhận";

        [Required]
        [StringLength(100)]
        public string HoTenNguoiNhan { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string SoDienThoai { get; set; } = string.Empty;

        [Required]
        public string DiaChiGiaoHang { get; set; } = string.Empty;

        public string? GhiChu { get; set; }

        public int? NguoiDungId { get; set; }

        public NguoiDung? NguoiDung { get; set; }

        public ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();
    }
}
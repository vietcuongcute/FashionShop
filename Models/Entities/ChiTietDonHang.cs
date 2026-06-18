using System.ComponentModel.DataAnnotations.Schema;

namespace FashionShop.Web.Models.Entities
{
    public class ChiTietDonHang
    {
        public int Id { get; set; }

        public int DonHangId { get; set; }

        public DonHang? DonHang { get; set; }

        public int SanPhamId { get; set; }

        public SanPham? SanPham { get; set; }

        public int SoLuong { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal DonGia { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ThanhTien { get; set; }
    }
}
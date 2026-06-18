using FashionShop.Web.Models.Entities;

namespace FashionShop.Web.Models.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int TongSanPham { get; set; }

        public int TongDanhMuc { get; set; }

        public int TongDonHang { get; set; }

        public int TongKhachHang { get; set; }

        public decimal TongDoanhThu { get; set; }

        public List<DonHang> DonHangMoiNhat { get; set; } = new List<DonHang>();

        public List<SanPham> SanPhamSapHetHang { get; set; } = new List<SanPham>();
    }
}
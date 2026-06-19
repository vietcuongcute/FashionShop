using FashionShop.Web.Models.Entities;

namespace FashionShop.Web.Models.ViewModels
{
    public class HomeViewModel
    {
        public List<DanhMuc> DanhMucs { get; set; } = new();

        public List<SanPham> SanPhamNoiBat { get; set; } = new();

        public List<SanPham> SanPhamMoi { get; set; } = new();

        public int TongSanPham { get; set; }

        public int TongDanhMuc { get; set; }
    }
}

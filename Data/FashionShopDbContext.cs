using FashionShop.Web.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace FashionShop.Web.Data
{
    public class FashionShopDbContext : DbContext
    {
        public FashionShopDbContext(DbContextOptions<FashionShopDbContext> options)
            : base(options)
        {
        }

        public DbSet<DanhMuc> DanhMucs { get; set; }
        public DbSet<SanPham> SanPhams { get; set; }
        public DbSet<NguoiDung> NguoiDungs { get; set; }
        public DbSet<DonHang> DonHangs { get; set; }
        public DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DanhMuc>().HasData(
                new DanhMuc { Id = 1, TenDanhMuc = "Áo thun", MoTa = "Áo thun basic, oversize và local brand dễ phối đồ" },
                new DanhMuc { Id = 2, TenDanhMuc = "Áo sơ mi", MoTa = "Áo sơ mi đi học, đi làm, phong cách Hàn Quốc" },
                new DanhMuc { Id = 3, TenDanhMuc = "Quần jean", MoTa = "Jean ống rộng, baggy, straight-fit thời trang" },
                new DanhMuc { Id = 4, TenDanhMuc = "Váy", MoTa = "Váy nữ, chân váy, đầm đi chơi và đi tiệc" },
                new DanhMuc { Id = 5, TenDanhMuc = "Áo khoác", MoTa = "Áo khoác bomber, hoodie, blazer nam nữ" },
                new DanhMuc { Id = 6, TenDanhMuc = "Phụ kiện", MoTa = "Túi, nón, thắt lưng và phụ kiện phối đồ" },
                new DanhMuc { Id = 7, TenDanhMuc = "Set đồ", MoTa = "Set phối sẵn cho đi học, đi làm, đi chơi" },
                new DanhMuc { Id = 8, TenDanhMuc = "Sale", MoTa = "Sản phẩm khuyến mãi nổi bật trong tuần" }
            );

            modelBuilder.Entity<SanPham>().HasData(
                new SanPham
                {
                    Id = 1,
                    TenSanPham = "Áo thun basic trắng",
                    Gia = 150000,
                    SoLuongTon = 50,
                    MoTa = "Áo thun cotton mềm, form regular, dễ phối với jean hoặc kaki.",
                    HinhAnh = "/images/products/ao-thun-trang.jpg",
                    NoiBat = true,
                    NgayTao = new DateTime(2026, 1, 1),
                    DanhMucId = 1
                },
                new SanPham
                {
                    Id = 2,
                    TenSanPham = "Áo sơ mi xanh pastel",
                    Gia = 250000,
                    SoLuongTon = 30,
                    MoTa = "Áo sơ mi xanh pastel trẻ trung, phù hợp đi học, đi làm và chụp ảnh.",
                    HinhAnh = "/images/products/ao-so-mi-xanh.jpg",
                    NoiBat = true,
                    NgayTao = new DateTime(2026, 1, 2),
                    DanhMucId = 2
                },
                new SanPham
                {
                    Id = 3,
                    TenSanPham = "Quần jean ống rộng",
                    Gia = 390000,
                    SoLuongTon = 25,
                    MoTa = "Quần jean ống rộng form đẹp, phong cách Hàn Quốc, dễ phối sneaker.",
                    HinhAnh = "/images/products/quan-jean-ong-rong.jpg",
                    NoiBat = true,
                    NgayTao = new DateTime(2026, 1, 3),
                    DanhMucId = 3
                },
                new SanPham { Id = 4, TenSanPham = "Hoodie nỉ form rộng", Gia = 420000, SoLuongTon = 18, MoTa = "Hoodie nỉ dày vừa, mũ đứng form, hợp thời tiết se lạnh.", HinhAnh = "/images/products/hoodie-form-rong.svg", NoiBat = true, NgayTao = new DateTime(2026, 2, 1), DanhMucId = 5 },
                new SanPham { Id = 5, TenSanPham = "Blazer đen thanh lịch", Gia = 590000, SoLuongTon = 12, MoTa = "Blazer dáng suông, phối được với áo thun, sơ mi hoặc váy.", HinhAnh = "/images/products/blazer-den.svg", NoiBat = true, NgayTao = new DateTime(2026, 2, 3), DanhMucId = 5 },
                new SanPham { Id = 6, TenSanPham = "Chân váy chữ A kem", Gia = 320000, SoLuongTon = 20, MoTa = "Chân váy chữ A tôn dáng, chất vải đứng form, dễ phối áo sơ mi.", HinhAnh = "/images/products/chan-vay-chu-a.svg", NoiBat = false, NgayTao = new DateTime(2026, 2, 5), DanhMucId = 4 },
                new SanPham { Id = 7, TenSanPham = "Set áo croptop và quần suông", Gia = 520000, SoLuongTon = 15, MoTa = "Set đồ phối sẵn năng động, phù hợp đi chơi cuối tuần.", HinhAnh = "/images/products/set-croptop-quan-suong.svg", NoiBat = true, NgayTao = new DateTime(2026, 3, 1), DanhMucId = 7 },
                new SanPham { Id = 8, TenSanPham = "Túi tote canvas", Gia = 180000, SoLuongTon = 45, MoTa = "Túi tote canvas dày, đựng laptop nhỏ, sách vở và phụ kiện hằng ngày.", HinhAnh = "/images/products/tui-tote-canvas.svg", NoiBat = false, NgayTao = new DateTime(2026, 3, 2), DanhMucId = 6 },
                new SanPham { Id = 9, TenSanPham = "Áo thun oversize graphic", Gia = 230000, SoLuongTon = 36, MoTa = "Áo thun oversize in graphic tối giản, chất cotton co giãn nhẹ.", HinhAnh = "/images/products/ao-thun-graphic.svg", NoiBat = true, NgayTao = new DateTime(2026, 3, 4), DanhMucId = 1 },
                new SanPham { Id = 10, TenSanPham = "Quần kaki ống suông", Gia = 360000, SoLuongTon = 22, MoTa = "Quần kaki ống suông màu be, phù hợp style tối giản và lịch sự.", HinhAnh = "/images/products/quan-kaki-suong.svg", NoiBat = false, NgayTao = new DateTime(2026, 3, 8), DanhMucId = 3 },
                new SanPham { Id = 11, TenSanPham = "Đầm midi hoa nhí", Gia = 450000, SoLuongTon = 14, MoTa = "Đầm midi hoa nhí nhẹ nhàng, hợp đi cafe, dạo phố hoặc du lịch.", HinhAnh = "/images/products/dam-midi-hoa.svg", NoiBat = true, NgayTao = new DateTime(2026, 3, 12), DanhMucId = 4 },
                new SanPham { Id = 12, TenSanPham = "Mũ bucket basic", Gia = 160000, SoLuongTon = 40, MoTa = "Mũ bucket basic chống nắng nhẹ, phối cùng streetwear hoặc casual.", HinhAnh = "/images/products/mu-bucket.svg", NoiBat = false, NgayTao = new DateTime(2026, 3, 16), DanhMucId = 6 }
            );

            modelBuilder.Entity<NguoiDung>().HasData(
                new NguoiDung
                {
                    Id = 1,
                    HoTen = "Quản trị viên",
                    Email = "admin@gmail.com",
                    MatKhau = "123456",
                    SoDienThoai = "0900000000",
                    DiaChi = "Đà Nẵng",
                    VaiTro = "Admin"
                }
            );
        }
    }
}

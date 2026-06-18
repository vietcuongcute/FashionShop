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
                new DanhMuc { Id = 1, TenDanhMuc = "Áo thun", MoTa = "Các mẫu áo thun thời trang" },
                new DanhMuc { Id = 2, TenDanhMuc = "Áo sơ mi", MoTa = "Áo sơ mi nam nữ" },
                new DanhMuc { Id = 3, TenDanhMuc = "Quần jean", MoTa = "Quần jean thời trang" },
                new DanhMuc { Id = 4, TenDanhMuc = "Váy", MoTa = "Váy nữ thời trang" },
                new DanhMuc { Id = 5, TenDanhMuc = "Áo khoác", MoTa = "Áo khoác nam nữ" }
            );

            modelBuilder.Entity<SanPham>().HasData(
                new SanPham
                {
                    Id = 1,
                    TenSanPham = "Áo thun basic trắng",
                    Gia = 150000,
                    SoLuongTon = 50,
                    MoTa = "Áo thun basic dễ phối đồ, chất liệu cotton thoáng mát.",
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
                    MoTa = "Áo sơ mi phong cách trẻ trung, phù hợp đi học, đi làm.",
                    HinhAnh = "/images/products/ao-so-mi-xanh.jpg",
                    NoiBat = true,
                    NgayTao = new DateTime(2026, 1, 1),
                    DanhMucId = 2
                },
                new SanPham
                {
                    Id = 3,
                    TenSanPham = "Quần jean ống rộng",
                    Gia = 390000,
                    SoLuongTon = 25,
                    MoTa = "Quần jean ống rộng phong cách Hàn Quốc.",
                    HinhAnh = "/images/products/quan-jean-ong-rong.jpg",
                    NoiBat = true,
                    NgayTao = new DateTime(2026, 1, 1),
                    DanhMucId = 3
                }
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
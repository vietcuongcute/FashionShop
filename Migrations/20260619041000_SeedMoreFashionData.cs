using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814

namespace FashionShop.Web.Migrations
{
    public partial class SeedMoreFashionData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "DanhMucs",
                keyColumn: "Id",
                keyValue: 1,
                column: "MoTa",
                value: "Áo thun basic, oversize và local brand dễ phối đồ");

            migrationBuilder.UpdateData(
                table: "DanhMucs",
                keyColumn: "Id",
                keyValue: 2,
                column: "MoTa",
                value: "Áo sơ mi đi học, đi làm, phong cách Hàn Quốc");

            migrationBuilder.UpdateData(
                table: "DanhMucs",
                keyColumn: "Id",
                keyValue: 3,
                column: "MoTa",
                value: "Jean ống rộng, baggy, straight-fit thời trang");

            migrationBuilder.UpdateData(
                table: "DanhMucs",
                keyColumn: "Id",
                keyValue: 4,
                column: "MoTa",
                value: "Váy nữ, chân váy, đầm đi chơi và đi tiệc");

            migrationBuilder.UpdateData(
                table: "DanhMucs",
                keyColumn: "Id",
                keyValue: 5,
                column: "MoTa",
                value: "Áo khoác bomber, hoodie, blazer nam nữ");

            migrationBuilder.InsertData(
                table: "DanhMucs",
                columns: new[] { "Id", "MoTa", "TenDanhMuc" },
                values: new object[,]
                {
                    { 6, "Túi, nón, thắt lưng và phụ kiện phối đồ", "Phụ kiện" },
                    { 7, "Set phối sẵn cho đi học, đi làm, đi chơi", "Set đồ" },
                    { 8, "Sản phẩm khuyến mãi nổi bật trong tuần", "Sale" }
                });

            migrationBuilder.UpdateData(
                table: "SanPhams",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "MoTa", "NgayTao" },
                values: new object[] { "Áo thun cotton mềm, form regular, dễ phối với jean hoặc kaki.", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "SanPhams",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "MoTa", "NgayTao" },
                values: new object[] { "Áo sơ mi xanh pastel trẻ trung, phù hợp đi học, đi làm và chụp ảnh.", new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "SanPhams",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "MoTa", "NgayTao" },
                values: new object[] { "Quần jean ống rộng form đẹp, phong cách Hàn Quốc, dễ phối sneaker.", new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "SanPhams",
                columns: new[] { "Id", "DanhMucId", "Gia", "HinhAnh", "MoTa", "NgayTao", "NoiBat", "SoLuongTon", "TenSanPham" },
                values: new object[,]
                {
                    { 4, 5, 420000m, "/images/products/hoodie-form-rong.svg", "Hoodie nỉ dày vừa, mũ đứng form, hợp thời tiết se lạnh.", new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 18, "Hoodie nỉ form rộng" },
                    { 5, 5, 590000m, "/images/products/blazer-den.svg", "Blazer dáng suông, phối được với áo thun, sơ mi hoặc váy.", new DateTime(2026, 2, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 12, "Blazer đen thanh lịch" },
                    { 6, 4, 320000m, "/images/products/chan-vay-chu-a.svg", "Chân váy chữ A tôn dáng, chất vải đứng form, dễ phối áo sơ mi.", new DateTime(2026, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 20, "Chân váy chữ A kem" },
                    { 7, 7, 520000m, "/images/products/set-croptop-quan-suong.svg", "Set đồ phối sẵn năng động, phù hợp đi chơi cuối tuần.", new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 15, "Set áo croptop và quần suông" },
                    { 8, 6, 180000m, "/images/products/tui-tote-canvas.svg", "Túi tote canvas dày, đựng laptop nhỏ, sách vở và phụ kiện hằng ngày.", new DateTime(2026, 3, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 45, "Túi tote canvas" },
                    { 9, 1, 230000m, "/images/products/ao-thun-graphic.svg", "Áo thun oversize in graphic tối giản, chất cotton co giãn nhẹ.", new DateTime(2026, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 36, "Áo thun oversize graphic" },
                    { 10, 3, 360000m, "/images/products/quan-kaki-suong.svg", "Quần kaki ống suông màu be, phù hợp style tối giản và lịch sự.", new DateTime(2026, 3, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 22, "Quần kaki ống suông" },
                    { 11, 4, 450000m, "/images/products/dam-midi-hoa.svg", "Đầm midi hoa nhí nhẹ nhàng, hợp đi cafe, dạo phố hoặc du lịch.", new DateTime(2026, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 14, "Đầm midi hoa nhí" },
                    { 12, 6, 160000m, "/images/products/mu-bucket.svg", "Mũ bucket basic chống nắng nhẹ, phối cùng streetwear hoặc casual.", new DateTime(2026, 3, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 40, "Mũ bucket basic" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(table: "SanPhams", keyColumn: "Id", keyValues: new object[] { 4, 5, 6, 7, 8, 9, 10, 11, 12 });
            migrationBuilder.DeleteData(table: "DanhMucs", keyColumn: "Id", keyValues: new object[] { 6, 7, 8 });
        }
    }
}

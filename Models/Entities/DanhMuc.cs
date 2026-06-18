using System.ComponentModel.DataAnnotations;

namespace FashionShop.Web.Models.Entities
{
    public class DanhMuc
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên danh mục không được để trống")]
        [StringLength(100)]
        public string TenDanhMuc { get; set; } = string.Empty;

        public string? MoTa { get; set; }

        public ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();
    }
}
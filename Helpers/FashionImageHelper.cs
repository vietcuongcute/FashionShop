using System;

namespace FashionShop.Web.Helpers
{
    public static class FashionImageHelper
    {
        public static string GetProductImage(string? imagePath, string? productName, string? categoryName = null)
        {
            if (!string.IsNullOrWhiteSpace(imagePath) && !imagePath.EndsWith(".svg", StringComparison.OrdinalIgnoreCase))
            {
                return imagePath;
            }

            var key = $"{productName} {categoryName}".ToLowerInvariant();

            if (key.Contains("sơ mi")) return "/images/products/ao-so-mi-xanh.jpg";
            if (key.Contains("áo thun") || key.Contains("tank") || key.Contains("hoodie")) return "/images/products/ao-thun-trang.jpg";
            if (key.Contains("quần") || key.Contains("jean") || key.Contains("kaki")) return "/images/products/quan-jean-ong-rong.jpg";
            if (key.Contains("váy") || key.Contains("đầm")) return "https://images.unsplash.com/photo-1496747611176-843222e1e57c?auto=format&fit=crop&w=900&q=80";
            if (key.Contains("blazer") || key.Contains("khoác") || key.Contains("outerwear")) return "https://images.unsplash.com/photo-1515886657613-9f3515b0c78f?auto=format&fit=crop&w=900&q=80";
            if (key.Contains("túi") || key.Contains("phụ kiện") || key.Contains("mũ")) return "https://images.unsplash.com/photo-1584917865442-de89df76afd3?auto=format&fit=crop&w=900&q=80";
            if (key.Contains("set")) return "https://images.unsplash.com/photo-1483985988355-763728e1935b?auto=format&fit=crop&w=900&q=80";

            return "https://images.unsplash.com/photo-1529139574466-a303027c1d8b?auto=format&fit=crop&w=900&q=80";
        }

        public static string GetCategoryImage(string? categoryName)
        {
            var key = (categoryName ?? string.Empty).ToLowerInvariant();

            if (key.Contains("áo")) return "https://images.unsplash.com/photo-1434389677669-e08b4cac3105?auto=format&fit=crop&w=900&q=80";
            if (key.Contains("quần")) return "https://images.unsplash.com/photo-1541099649105-f69ad21f3246?auto=format&fit=crop&w=900&q=80";
            if (key.Contains("váy") || key.Contains("đầm")) return "https://images.unsplash.com/photo-1496747611176-843222e1e57c?auto=format&fit=crop&w=900&q=80";
            if (key.Contains("khoác") || key.Contains("blazer")) return "https://images.unsplash.com/photo-1515886657613-9f3515b0c78f?auto=format&fit=crop&w=900&q=80";
            if (key.Contains("phụ kiện") || key.Contains("túi")) return "https://images.unsplash.com/photo-1584917865442-de89df76afd3?auto=format&fit=crop&w=900&q=80";
            if (key.Contains("sale")) return "https://images.unsplash.com/photo-1483985988355-763728e1935b?auto=format&fit=crop&w=900&q=80";
            return "https://images.unsplash.com/photo-1445205170230-053b83016050?auto=format&fit=crop&w=900&q=80";
        }

        public static string GetLookbook(string key) => key switch
        {
            "hero" => "https://images.unsplash.com/photo-1529139574466-a303027c1d8b?auto=format&fit=crop&w=1400&q=80",
            "hero-side" => "https://images.unsplash.com/photo-1483985988355-763728e1935b?auto=format&fit=crop&w=1000&q=80",
            "collection" => "https://images.unsplash.com/photo-1445205170230-053b83016050?auto=format&fit=crop&w=1200&q=80",
            "occasion" => "https://images.unsplash.com/photo-1496747611176-843222e1e57c?auto=format&fit=crop&w=1200&q=80",
            "outerwear" => "https://images.unsplash.com/photo-1515886657613-9f3515b0c78f?auto=format&fit=crop&w=1200&q=80",
            "login" => "https://images.unsplash.com/photo-1487412720507-e7ab37603c6f?auto=format&fit=crop&w=1200&q=80",
            "register" => "https://images.unsplash.com/photo-1524504388940-b1c1722653e1?auto=format&fit=crop&w=1200&q=80",
            "shop" => "https://images.unsplash.com/photo-1441986300917-64674bd600d8?auto=format&fit=crop&w=1400&q=80",
            _ => "https://images.unsplash.com/photo-1483985988355-763728e1935b?auto=format&fit=crop&w=1200&q=80"
        };
    }
}

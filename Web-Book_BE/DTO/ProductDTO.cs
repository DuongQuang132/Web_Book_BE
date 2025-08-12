using Microsoft.AspNetCore.Http;

namespace Web_Book_BE.DTO
{
    // Dùng khi tạo mới sản phẩm
    public class ProductCreateDTO
    {
        public string ProductName { get; set; }
        public string AuthorName { get; set; }
        public string CategoriesName { get; set; }
        public decimal? Price { get; set; }
        public string? Discount { get; set; }
        public string Description { get; set; }
        public int? Quantity { get; set; }
        public IFormFile? Image { get; set; } // File upload (optional)
        public string? ImageUrl { get; set; } // URL ảnh (optional)
    }

    // Dùng khi cập nhật sản phẩm
    public class ProductUpdateDTO
    {
        public string ProductId { get; set; } // Để xác định sản phẩm cần cập nhật
        public string ProductName { get; set; }
        public string AuthorName { get; set; }
        public string CategoriesName { get; set; }
        public decimal? Price { get; set; }
        public string? Discount { get; set; }
        public string Description { get; set; }
        public int? Quantity { get; set; }
        public IFormFile? Image { get; set; } // File upload (optional)
        public string? ImageUrl { get; set; } // URL ảnh (optional)
    }

    // Dùng để trả về dữ liệu sản phẩm cho client
    public class ProductResponseDTO
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string AuthorName { get; set; }
        public string CategoriesName { get; set; }
        public decimal? Price { get; set; }
        public string? Discount { get; set; }
        public string Description { get; set; }
        public int? Quantity { get; set; }
        public string ImageUrl { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    // Dùng để tìm kiếm hoặc lọc sản phẩm
    public class ProductFilterDTO
    {
        public string? Keyword { get; set; }
        public string? AuthorId { get; set; }
        public string? CategoriesId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public bool? OnlyDiscounted { get; set; }
        public bool? OnlyAvailable { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    // Dùng để xóa mềm sản phẩm
    public class ProductDeleteDTO
    {
        public string ProductId { get; set; }
    }
}

namespace Web_Book_BE.DTO
{
    // Dùng khi tạo mới thông tin chi tiết sản phẩm
    public class ProductDetailCreateDTO
    {
        public string ProductId { get; set; }
        public string Publisher { get; set; }
        public DateTime? PublishDate { get; set; }
        public int? PageCount { get; set; }
        public string Language { get; set; }
    }

    // Dùng khi cập nhật thông tin chi tiết sản phẩm
    public class ProductDetailUpdateDTO
    {
        public string ProductDetailId { get; set; }
        public string Publisher { get; set; }
        public DateTime? PublishDate { get; set; }
        public int? PageCount { get; set; }
        public string Language { get; set; }
    }

    // Dùng để trả về thông tin chi tiết sản phẩm cho client
    public class ProductDetailResponseDTO
    {
        public string ProductDetailId { get; set; }
        public string ProductId { get; set; }
        public string Publisher { get; set; }
        public DateTime? PublishDate { get; set; }
        public int? PageCount { get; set; }
        public string Language { get; set; }
    }

    // Dùng để truy vấn chi tiết theo Product_ID
    public class ProductDetailByProductDTO
    {
        public string ProductId { get; set; }
    }
}

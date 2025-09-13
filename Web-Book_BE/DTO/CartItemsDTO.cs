namespace Web_Book_BE.DTO
{
    // Dùng khi thêm sản phẩm vào giỏ hàng
    public class CartItemCreateDTO
    {
        public string UserId { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }

    // Dùng khi cập nhật số lượng sản phẩm trong giỏ
    public class CartItemUpdateDTO
    {
        public string CartItemId { get; set; }
        public int Quantity { get; set; }
    }

    // Dùng để trả về dữ liệu giỏ hàng cho client
    public class CartItemResponseDTO
    {
        public string CartItemId { get; set; }
        public string UserId { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; } 
        public string ImageUrl { get; set; }
        public decimal? Price { get; set; }
        public int Quantity { get; set; }
        public decimal? TotalPrice { get; set; } 
    }

    // Dùng để xóa sản phẩm khỏi giỏ
    public class CartItemDeleteDTO
    {
        public string CartItemId { get; set; }
    }

    // Dùng để truy vấn giỏ hàng theo User
    public class CartItemByUserDTO
    {
        public string UserId { get; set; }
    }
}

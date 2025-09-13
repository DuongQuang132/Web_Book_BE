namespace Web_Book_BE.DTO
{
    // Dùng khi tạo đơn hàng mới
    public class OrderCreateDTO
    {
        public string UserId { get; set; }
        public decimal? TotalAmount { get; set; }
        public string Status { get; set; }
        public string ShippingAddress { get; set; }
    }

    // Dùng khi cập nhật đơn hàng
    public class OrderUpdateDTO
    {
        public string OrdersId { get; set; }
        public string Status { get; set; }
 
    }

    // Dùng để cập nhật trạng thái đơn hàng riêng biệt
    public class OrderStatusUpdateDTO
    {
        public string OrdersId { get; set; }
        public string Status { get; set; }
    }

    // Dùng để trả về dữ liệu đơn hàng cho client
    public class OrderResponseDTO
    {
        public string OrdersId { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public decimal? TotalAmount { get; set; }
        public string Status { get; set; }
        public string ShippingAddress { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    // Dùng để truy vấn đơn hàng theo người dùng
    public class OrderByUserDTO
    {
        public string UserId { get; set; }
    }

    // Dùng để truy vấn đơn hàng theo ID
    public class OrderByIdDTO
    {
        public string OrdersId { get; set; }
    }
}

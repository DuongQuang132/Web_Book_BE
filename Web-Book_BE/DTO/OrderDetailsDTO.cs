namespace Web_Book_BE.DTO
{
    // Dùng khi thêm sản phẩm vào đơn hàng
    public class OrderDetailCreateDTO
    {
        public string OrdersId { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    // Dùng khi cập nhật chi tiết đơn hàng
    public class OrderDetailUpdateDTO
    {
        public string OrderDetailId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    // Dùng để trả về dữ liệu chi tiết đơn hàng cho client
    public class OrderDetailResponseDTO
    {
        public string OrderDetailId { get; set; }
        public string OrdersId { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; } // Quantity × UnitPrice
    }

    // Dùng để truy vấn chi tiết đơn hàng theo Orders_ID
    public class OrderDetailByOrderDTO
    {
        public string OrdersId { get; set; }
    }
}

namespace Web_Book_BE.DTO
{
    // Dùng khi tạo thông tin thanh toán
    public class PaymentCreateDTO
    {
        public string OrdersId { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal? AmountPaid { get; set; }
        public string Status { get; set; }
        public string Noted { get; set; }
    }

    // Dùng khi cập nhật thông tin thanh toán
    public class PaymentUpdateDTO
    {
        public string PaymentId { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal? AmountPaid { get; set; }
        public string Status { get; set; }
        public string Noted { get; set; }
    }

    // Dùng để cập nhật trạng thái thanh toán riêng biệt
    public class PaymentStatusUpdateDTO
    {
        public string PaymentId { get; set; }
        public string Status { get; set; }
    }

    // Dùng để trả về dữ liệu thanh toán cho client
    public class PaymentResponseDTO
    {
        public string PaymentId { get; set; }
        public string OrdersId { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal? AmountPaid { get; set; }
        public string Status { get; set; }
        public string Noted { get; set; }
    }

    // Dùng để truy vấn thanh toán theo Orders_ID
    public class PaymentByOrderDTO
    {
        public string OrdersId { get; set; }
    }
}

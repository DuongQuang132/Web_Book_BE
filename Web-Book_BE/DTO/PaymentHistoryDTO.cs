namespace Web_Book_BE.DTO
{
    // Dùng khi tạo bản ghi lịch sử thanh toán
    public class PaymentHistoryCreateDTO
    {
        public string PaymentId { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal Amount { get; set; }
    }

    // Dùng để trả về dữ liệu lịch sử thanh toán cho client
    public class PaymentHistoryResponseDTO
    {
        public string PaymentHistoryId { get; set; }
        public string PaymentId { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal Amount { get; set; }
    }

    // Dùng để truy vấn lịch sử theo Payment_ID
    public class PaymentHistoryByPaymentDTO
    {
        public string PaymentId { get; set; }
    }

    // Dùng để lọc lịch sử theo ngày
    public class PaymentHistoryFilterDTO
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}

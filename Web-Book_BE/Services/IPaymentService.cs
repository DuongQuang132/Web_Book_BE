using Web_Book_BE.DTO;
using Web_Book_BE.Models;
using Web_Book_BE.Services.Interfaces;
using Web_Book_BE.Utils;

namespace Web_Book_BE.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly BookStoreDbContext _context;

        public PaymentService(BookStoreDbContext context)
        {
            _context = context;
        }

        public string CreatePayment(PaymentCreateDTO dto)
        {
            if (string.IsNullOrEmpty(dto.OrdersId) || string.IsNullOrEmpty(dto.PaymentMethod))
                throw new ArgumentException("OrdersId và phương thức thanh toán là bắt buộc");

            var payment = new Payment
            {
                PaymentId = "PAY" + IdGenerator.RandomDigits(),
                OrdersId = dto.OrdersId,
                PaymentMethod = dto.PaymentMethod,
                PaymentDate = dto.PaymentDate ?? DateTime.UtcNow,
                AmountPaid = dto.AmountPaid,
                Status = dto.Status ?? "Pending",
                Noted = dto.Noted
            };

            _context.Payments.Add(payment);
            _context.SaveChanges();
            return "Thông tin thanh toán đã được tạo";
        }

        public string UpdatePayment(PaymentUpdateDTO dto)
        {
            var payment = _context.Payments.FirstOrDefault(p => p.PaymentId == dto.PaymentId);
            if (payment == null) throw new KeyNotFoundException("Không tìm thấy thông tin thanh toán");

            payment.PaymentMethod = dto.PaymentMethod;
            payment.PaymentDate = dto.PaymentDate ?? payment.PaymentDate;
            payment.AmountPaid = dto.AmountPaid;
            payment.Status = dto.Status;
            payment.Noted = dto.Noted;

            _context.SaveChanges();
            return "Thông tin thanh toán đã được cập nhật";
        }

        public string UpdatePaymentStatus(PaymentStatusUpdateDTO dto)
        {
            var payment = _context.Payments.FirstOrDefault(p => p.PaymentId == dto.PaymentId);
            if (payment == null) throw new KeyNotFoundException("Không tìm thấy thông tin thanh toán");

            payment.Status = dto.Status;
            _context.SaveChanges();
            return "Trạng thái thanh toán đã được cập nhật";
        }

        public string DeletePayment(string id)
        {
            var payment = _context.Payments.FirstOrDefault(p => p.PaymentId == id);
            if (payment == null) throw new KeyNotFoundException("Không tìm thấy thông tin thanh toán để xóa");

            _context.Payments.Remove(payment);
            _context.SaveChanges();
            return "Thông tin thanh toán đã được xóa";
        }

        public List<PaymentResponseDTO> GetPaymentsByOrder(string ordersId)
        {
            return _context.Payments
                .Where(p => p.OrdersId == ordersId)
                .Select(p => new PaymentResponseDTO
                {
                    PaymentId = p.PaymentId,
                    OrdersId = p.OrdersId ?? "",
                    PaymentMethod = p.PaymentMethod ?? "",
                    PaymentDate = p.PaymentDate,
                    AmountPaid = p.AmountPaid,
                    Status = p.Status ?? "",
                    Noted = p.Noted ?? ""
                }).ToList();
        }

        public List<PaymentResponseDTO> GetAllPayments()
        {
            return _context.Payments
                .Select(p => new PaymentResponseDTO
                {
                    PaymentId = p.PaymentId,
                    OrdersId = p.OrdersId ?? "",
                    PaymentMethod = p.PaymentMethod ?? "",
                    PaymentDate = p.PaymentDate,
                    AmountPaid = p.AmountPaid,
                    Status = p.Status ?? "",
                    Noted = p.Noted ?? ""
                }).ToList();
        }
    }
}
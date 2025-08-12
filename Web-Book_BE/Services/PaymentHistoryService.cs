using Web_Book_BE.DTO;
using Web_Book_BE.Models;
using Web_Book_BE.Services.Interfaces;
using Web_Book_BE.Utils;

namespace Web_Book_BE.Services
{
    public class PaymentHistoryService : IPaymentHistoryService
    {
        private readonly BookStoreDbContext _context;

        public PaymentHistoryService(BookStoreDbContext context)
        {
            _context = context;
        }

        public string CreatePaymentHistory(PaymentHistoryCreateDTO dto)
        {
            if (string.IsNullOrEmpty(dto.PaymentId))
                throw new ArgumentException("PaymentId là bắt buộc");

            var history = new PaymentHistory
            {
                PaymentHistoryId = "PHIS" + IdGenerator.RandomDigits(),
                PaymentId = dto.PaymentId,
                PaymentDate = dto.PaymentDate ?? DateTime.UtcNow,
                Amount = dto.Amount
            };

            _context.PaymentHistories.Add(history);
            _context.SaveChanges();

            return "Lịch sử thanh toán đã được tạo";
        }

        public List<PaymentHistoryResponseDTO> GetHistoryByPayment(string paymentId)
        {
            return _context.PaymentHistories
                .Where(h => h.PaymentId == paymentId)
                .Select(h => new PaymentHistoryResponseDTO
                {
                    PaymentHistoryId = h.PaymentHistoryId,
                    PaymentId = h.PaymentId ?? "",
                    PaymentDate = h.PaymentDate,
                    Amount = h.Amount ?? 0
                }).ToList();
        }

        public List<PaymentHistoryResponseDTO> FilterHistoryByDate(PaymentHistoryFilterDTO dto)
        {
            var query = _context.PaymentHistories.AsQueryable();

            if (dto.FromDate.HasValue)
                query = query.Where(h => h.PaymentDate >= dto.FromDate.Value);

            if (dto.ToDate.HasValue)
                query = query.Where(h => h.PaymentDate <= dto.ToDate.Value);

            return query
                .Select(h => new PaymentHistoryResponseDTO
                {
                    PaymentHistoryId = h.PaymentHistoryId,
                    PaymentId = h.PaymentId ?? "",
                    PaymentDate = h.PaymentDate,
                    Amount = h.Amount ?? 0
                }).ToList();
        }

        public List<PaymentHistoryResponseDTO> GetAllPaymentHistories()
        {
            return _context.PaymentHistories
                .Select(h => new PaymentHistoryResponseDTO
                {
                    PaymentHistoryId = h.PaymentHistoryId,
                    PaymentId = h.PaymentId ?? "",
                    PaymentDate = h.PaymentDate,
                    Amount = h.Amount ?? 0
                }).ToList();
        }
    }
}

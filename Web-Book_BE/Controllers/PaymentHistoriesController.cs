using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Book_BE.DTO;
using Web_Book_BE.Models;
using Web_Book_BE.Utils;

namespace Web_Book_BE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentHistoryController : ControllerBase
    {
        private readonly BookStoreDbContext _context;

        public PaymentHistoryController(BookStoreDbContext context)
        {
            _context = context;
        }

        //Tạo bản ghi lịch sử thanh toán
        [HttpPost]
        public IActionResult CreatePaymentHistory([FromBody] PaymentHistoryCreateDTO dto)
        {
            if (string.IsNullOrEmpty(dto.PaymentId))
                return BadRequest("PaymentId là bắt buộc");

            var history = new PaymentHistory
            {
                PaymentHistoryId = "PHIS" + IdGenerator.RandomDigits(),
                PaymentId = dto.PaymentId,
                PaymentDate = dto.PaymentDate ?? DateTime.UtcNow,
                Amount = dto.Amount
            };

            _context.PaymentHistories.Add(history);
            _context.SaveChanges();

            return Ok("Lịch sử thanh toán đã được tạo");
        }

        //Lấy lịch sử theo PaymentId
        [HttpGet("payment/{paymentId}")]
        public IActionResult GetHistoryByPayment(string paymentId)
        {
            var histories = _context.PaymentHistories
                .Where(h => h.PaymentId == paymentId)
                .Select(h => new PaymentHistoryResponseDTO
                {
                    PaymentHistoryId = h.PaymentHistoryId,
                    PaymentId = h.PaymentId ?? "",
                    PaymentDate = h.PaymentDate,
                    Amount = h.Amount ?? 0
                })
                .ToList();

            return Ok(histories);
        }

        //Lọc lịch sử theo khoảng thời gian
        [HttpPost("filter")]
        public IActionResult FilterHistoryByDate([FromBody] PaymentHistoryFilterDTO dto)
        {
            var query = _context.PaymentHistories.AsQueryable();

            if (dto.FromDate.HasValue)
                query = query.Where(h => h.PaymentDate >= dto.FromDate.Value);

            if (dto.ToDate.HasValue)
                query = query.Where(h => h.PaymentDate <= dto.ToDate.Value);

            var histories = query
                .Select(h => new PaymentHistoryResponseDTO
                {
                    PaymentHistoryId = h.PaymentHistoryId,
                    PaymentId = h.PaymentId ?? "",
                    PaymentDate = h.PaymentDate,
                    Amount = h.Amount ?? 0
                })
                .ToList();

            return Ok(histories);
        }

        //Lấy tất cả lịch sử thanh toán
        [HttpGet]
        public IActionResult GetAllPaymentHistories()
        {
            var histories = _context.PaymentHistories
                .Select(h => new PaymentHistoryResponseDTO
                {
                    PaymentHistoryId = h.PaymentHistoryId,
                    PaymentId = h.PaymentId ?? "",
                    PaymentDate = h.PaymentDate,
                    Amount = h.Amount ?? 0
                })
                .ToList();

            return Ok(histories);
        }
    }
}

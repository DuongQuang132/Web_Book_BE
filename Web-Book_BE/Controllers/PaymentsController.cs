using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Book_BE.DTO;
using Web_Book_BE.Models;
using Web_Book_BE.Utils;

namespace Web_Book_BE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly BookStoreDbContext _context;

        public PaymentsController(BookStoreDbContext context)
        {
            _context = context;
        }

        //Tạo thông tin thanh toán
        [HttpPost]
        public IActionResult CreatePayment([FromBody] PaymentCreateDTO dto)
        {
            if (string.IsNullOrEmpty(dto.OrdersId) || string.IsNullOrEmpty(dto.PaymentMethod))
                return BadRequest("OrdersId và phương thức thanh toán là bắt buộc");

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

            return Ok("Thông tin thanh toán đã được tạo");
        }

        //Cập nhật thông tin thanh toán
        [HttpPut]
        public IActionResult UpdatePayment([FromBody] PaymentUpdateDTO dto)
        {
            var payment = _context.Payments.FirstOrDefault(p => p.PaymentId == dto.PaymentId);
            if (payment == null)
                return NotFound("Không tìm thấy thông tin thanh toán");

            payment.PaymentMethod = dto.PaymentMethod;
            payment.PaymentDate = dto.PaymentDate ?? payment.PaymentDate;
            payment.AmountPaid = dto.AmountPaid;
            payment.Status = dto.Status;
            payment.Noted = dto.Noted;

            _context.SaveChanges();
            return Ok("Thông tin thanh toán đã được cập nhật");
        }

        //Cập nhật trạng thái thanh toán riêng biệt
        [HttpPatch("status")]
        public IActionResult UpdatePaymentStatus([FromBody] PaymentStatusUpdateDTO dto)
        {
            var payment = _context.Payments.FirstOrDefault(p => p.PaymentId == dto.PaymentId);
            if (payment == null)
                return NotFound("Không tìm thấy thông tin thanh toán");

            payment.Status = dto.Status;
            _context.SaveChanges();

            return Ok("Trạng thái thanh toán đã được cập nhật");
        }

        //Xóa thanh toán
        [HttpDelete("{id}")]
        public IActionResult DeletePayment(string id)
        {
            var payment = _context.Payments.FirstOrDefault(p => p.PaymentId == id);
            if (payment == null)
                return NotFound("Không tìm thấy thông tin thanh toán để xóa");

            _context.Payments.Remove(payment);
            _context.SaveChanges();

            return Ok("Thông tin thanh toán đã được xóa");
        }

        //Lấy thanh toán theo OrdersId
        [HttpGet("order/{ordersId}")]
        public IActionResult GetPaymentsByOrder(string ordersId)
        {
            var payments = _context.Payments
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
                })
                .ToList();

            return Ok(payments);
        }

        //Lấy tất cả thanh toán
        [HttpGet]
        public IActionResult GetAllPayments()
        {
            var payments = _context.Payments
                .Select(p => new PaymentResponseDTO
                {
                    PaymentId = p.PaymentId,
                    OrdersId = p.OrdersId ?? "",
                    PaymentMethod = p.PaymentMethod ?? "",
                    PaymentDate = p.PaymentDate,
                    AmountPaid = p.AmountPaid,
                    Status = p.Status ?? "",
                    Noted = p.Noted ?? ""
                })
                .ToList();

            return Ok(payments);
        }
    }
}

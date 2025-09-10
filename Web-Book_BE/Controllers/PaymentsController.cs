using Microsoft.AspNetCore.Mvc;
using Web_Book_BE.DTO;
using Web_Book_BE.Services.Interfaces;

namespace Web_Book_BE.Controllers
{
    [ApiController]
    [Route(" payment")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost]
        public IActionResult CreatePayment([FromBody] PaymentCreateDTO dto)
        {
            try
            {
                var result = _paymentService.CreatePayment(dto);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IActionResult UpdatePayment([FromBody] PaymentUpdateDTO dto)
        {
            try
            {
                var result = _paymentService.UpdatePayment(dto);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPatch("status")]
        public IActionResult UpdatePaymentStatus([FromBody] PaymentStatusUpdateDTO dto)
        {
            try
            {
                var result = _paymentService.UpdatePaymentStatus(dto);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePayment(string id)
        {
            try
            {
                var result = _paymentService.DeletePayment(id);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("order/{ordersId}")]
        public IActionResult GetPaymentsByOrder(string ordersId)
        {
            var payments = _paymentService.GetPaymentsByOrder(ordersId);
            return Ok(payments);
        }

        [HttpGet]
        public IActionResult GetAllPayments()
        {
            var payments = _paymentService.GetAllPayments();
            return Ok(payments);
        }
    }
}
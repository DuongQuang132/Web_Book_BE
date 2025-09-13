using Microsoft.AspNetCore.Mvc;
using Web_Book_BE.Services.Interfaces;

namespace Web_Book_BE.Controllers
{
    public class PaymentRequest
    {
        public decimal Amount { get; set; }
    }

    [ApiController]
    [Route("paymentmomo")]
    public class PaymentController : ControllerBase
    {
        private readonly IMomoService _momoService;

        public PaymentController(IMomoService momoService)
        {
            _momoService = momoService;
        }

        [HttpPost("create-payment")]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentRequest request)
        {
            try
            {
                var result = await _momoService.CreatePaymentAsync(request.Amount);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
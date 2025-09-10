using Microsoft.AspNetCore.Mvc;
using Web_Book_BE.DTO;
using Web_Book_BE.Services.Interfaces;

namespace Web_Book_BE.Controllers
{
    [ApiController]
    [Route("paymenthistory")]
    public class PaymentHistoryController : ControllerBase
    {
        private readonly IPaymentHistoryService _paymentHistoryService;

        public PaymentHistoryController(IPaymentHistoryService paymentHistoryService)
        {
            _paymentHistoryService = paymentHistoryService;
        }

        [HttpPost]
        public IActionResult CreatePaymentHistory([FromBody] PaymentHistoryCreateDTO dto)
        {
            try
            {
                var result = _paymentHistoryService.CreatePaymentHistory(dto);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("payment/{paymentId}")]
        public IActionResult GetHistoryByPayment(string paymentId)
        {
            var histories = _paymentHistoryService.GetHistoryByPayment(paymentId);
            return Ok(histories);
        }

        [HttpPost("filter")]
        public IActionResult FilterHistoryByDate([FromBody] PaymentHistoryFilterDTO dto)
        {
            var histories = _paymentHistoryService.FilterHistoryByDate(dto);
            return Ok(histories);
        }

        [HttpGet]
        public IActionResult GetAllPaymentHistories()
        {
            var histories = _paymentHistoryService.GetAllPaymentHistories();
            return Ok(histories);
        }
    }
}
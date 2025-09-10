// Controllers/OrderDetailsController.cs
using Microsoft.AspNetCore.Mvc;
using Web_Book_BE.DTO;
using Web_Book_BE.Services.Interfaces;

namespace Web_Book_BE.Controllers
{
    [ApiController]
    [Route("orderdetail")]
    public class OrderDetailsController : ControllerBase
    {
        private readonly IOrderDetailsService _service;

        public OrderDetailsController(IOrderDetailsService service)
        {
            _service = service;
        }

        // Thêm chi tiết đơn hàng
        [HttpPost]
        public IActionResult CreateOrderDetail([FromBody] OrderDetailCreateDTO dto)
        {
            try
            {
                var id = _service.CreateOrderDetail(dto);
                return Ok($"Chi tiết đơn hàng đã được thêm với ID: {id}");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Cập nhật chi tiết đơn hàng
        [HttpPut]
        public IActionResult UpdateOrderDetail([FromBody] OrderDetailUpdateDTO dto)
        {
            var success = _service.UpdateOrderDetail(dto);
            if (!success)
                return NotFound("Không tìm thấy chi tiết đơn hàng");

            return Ok("Chi tiết đơn hàng đã được cập nhật");
        }

        // Xóa chi tiết đơn hàng
        [HttpDelete("{id}")]
        public IActionResult DeleteOrderDetail(string id)
        {
            var success = _service.DeleteOrderDetail(id);
            if (!success)
                return NotFound("Không tìm thấy chi tiết đơn hàng để xóa");

            return Ok("Chi tiết đơn hàng đã được xóa");
        }

        // Lấy danh sách chi tiết đơn hàng theo OrdersId
        [HttpGet("order/{ordersId}")]
        public IActionResult GetOrderDetailsByOrder(string ordersId)
        {
            var details = _service.GetOrderDetailsByOrder(ordersId);
            return Ok(details);
        }
    }
}

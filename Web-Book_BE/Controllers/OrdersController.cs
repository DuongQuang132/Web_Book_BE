using Microsoft.AspNetCore.Mvc;
using Web_Book_BE.DTO;
using Web_Book_BE.Services.Interfaces;

namespace Web_Book_BE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _service;

        public OrdersController(IOrderService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult CreateOrder([FromBody] OrderCreateDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.UserId) || dto.TotalAmount == null)
                return BadRequest("UserId và tổng tiền là bắt buộc");

            try
            {
                var orderId = _service.CreateOrder(dto);
                return Ok(new { Message = "Đơn hàng đã được tạo thành công", OrderId = orderId });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IActionResult UpdateOrder([FromBody] OrderUpdateDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.OrdersId))
                return BadRequest("OrdersId là bắt buộc");

            try
            {
                _service.UpdateOrder(dto);
                return Ok("Đơn hàng đã được cập nhật");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPatch("status")]
        public IActionResult UpdateOrderStatus([FromBody] OrderStatusUpdateDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.OrdersId))
                return BadRequest("OrdersId là bắt buộc");

            try
            {
                _service.UpdateOrderStatus(dto);
                return Ok("Trạng thái đơn hàng đã được cập nhật");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteOrder(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("OrdersId là bắt buộc");

            try
            {
                _service.DeleteOrder(id);
                return Ok("Đơn hàng đã được xóa");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetOrderById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("OrdersId là bắt buộc");

            try
            {
                var order = _service.GetOrderById(id);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("user/{userId}")]
        public IActionResult GetOrdersByUser(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest("UserId là bắt buộc");

            var orders = _service.GetOrdersByUser(userId);
            return Ok(orders);
        }

        [HttpGet]
        public IActionResult GetAllOrders()
        {
            var orders = _service.GetAllOrders();
            return Ok(orders);
        }
    }
}

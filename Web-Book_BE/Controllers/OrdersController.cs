using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Book_BE.DTO;
using Web_Book_BE.Models;
using Web_Book_BE.Utils;

namespace Web_Book_BE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly BookStoreDbContext _context;

        public OrdersController(BookStoreDbContext context)
        {
            _context = context;
        }

        // ✅ Tạo đơn hàng mới
        [HttpPost]
        public IActionResult CreateOrder([FromBody] OrderCreateDTO dto)
        {
            if (string.IsNullOrEmpty(dto.UserId) || dto.TotalAmount == null)
                return BadRequest("UserId và tổng tiền là bắt buộc");

            var order = new Orders
            {
                OrdersId = "ORD" + IdGenerator.RandomDigits(),
                UserId = dto.UserId,
                TotalAmount = dto.TotalAmount,
                Status = dto.Status ?? "Pending",
                ShippingAddress = dto.ShippingAddress,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            return Ok("Đơn hàng đã được tạo thành công");
        }

        //Cập nhật đơn hàng
        [HttpPut]
        public IActionResult UpdateOrder([FromBody] OrderUpdateDTO dto)
        {
            var order = _context.Orders.FirstOrDefault(o => o.OrdersId == dto.OrdersId);
            if (order == null)
                return NotFound("Không tìm thấy đơn hàng");

            order.TotalAmount = dto.TotalAmount;
            order.Status = dto.Status;
            order.ShippingAddress = dto.ShippingAddress;
            order.UpdatedAt = DateTime.UtcNow;

            _context.SaveChanges();
            return Ok("Đơn hàng đã được cập nhật");
        }

        //Cập nhật trạng thái đơn hàng riêng biệt
        [HttpPatch("status")]
        public IActionResult UpdateOrderStatus([FromBody] OrderStatusUpdateDTO dto)
        {
            var order = _context.Orders.FirstOrDefault(o => o.OrdersId == dto.OrdersId);
            if (order == null)
                return NotFound("Không tìm thấy đơn hàng");

            order.Status = dto.Status;
            order.UpdatedAt = DateTime.UtcNow;

            _context.SaveChanges();
            return Ok("Trạng thái đơn hàng đã được cập nhật");
        }

        //Xóa đơn hàng
        [HttpDelete("{id}")]
        public IActionResult DeleteOrder(string id)
        {
            var order = _context.Orders.FirstOrDefault(o => o.OrdersId == id);
            if (order == null)
                return NotFound("Không tìm thấy đơn hàng để xóa");

            _context.Orders.Remove(order);
            _context.SaveChanges();

            return Ok("Đơn hàng đã được xóa");
        }

        //Lấy đơn hàng theo ID
        [HttpGet("{id}")]
        public IActionResult GetOrderById(string id)
        {
            var order = _context.Orders
                .Include(o => o.User)
                .FirstOrDefault(o => o.OrdersId == id);

            if (order == null)
                return NotFound("Không tìm thấy đơn hàng");

            var response = new OrderResponseDTO
            {
                OrdersId = order.OrdersId,
                UserId = order.UserId,
                Username = order.User?.Username,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                ShippingAddress = order.ShippingAddress,
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt
            };

            return Ok(response);
        }

        //Lấy đơn hàng theo UserId
        [HttpGet("user/{userId}")]
        public IActionResult GetOrdersByUser(string userId)
        {
            var orders = _context.Orders
                .Include(o => o.User)
                .Where(o => o.UserId == userId)
                .Select(order => new OrderResponseDTO
                {
                    OrdersId = order.OrdersId,
                    UserId = order.UserId,
                    Username = order.User.Username,
                    TotalAmount = order.TotalAmount,
                    Status = order.Status,
                    ShippingAddress = order.ShippingAddress,
                    CreatedAt = order.CreatedAt,
                    UpdatedAt = order.UpdatedAt
                })
                .ToList();

            return Ok(orders);
        }

        //Lấy tất cả đơn hàng
        [HttpGet]
        public IActionResult GetAllOrders()
        {
            var orders = _context.Orders
                .Include(o => o.User)
                .Select(order => new OrderResponseDTO
                {
                    OrdersId = order.OrdersId,
                    UserId = order.UserId,
                    Username = order.User.Username,
                    TotalAmount = order.TotalAmount,
                    Status = order.Status,
                    ShippingAddress = order.ShippingAddress,
                    CreatedAt = order.CreatedAt,
                    UpdatedAt = order.UpdatedAt
                })
                .ToList();

            return Ok(orders);
        }
    }
}

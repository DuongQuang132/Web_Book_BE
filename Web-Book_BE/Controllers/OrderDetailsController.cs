using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Book_BE.DTO;
using Web_Book_BE.Models;
using Web_Book_BE.Utils;

namespace Web_Book_BE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderDetailsController : ControllerBase
    {
        private readonly BookStoreDbContext _context;

        public OrderDetailsController(BookStoreDbContext context)
        {
            _context = context;
        }

        //Thêm sản phẩm vào đơn hàng
        [HttpPost]
        public IActionResult CreateOrderDetail([FromBody] OrderDetailCreateDTO dto)
        {
            if (string.IsNullOrEmpty(dto.OrdersId) || string.IsNullOrEmpty(dto.ProductId))
                return BadRequest("OrdersId và ProductId là bắt buộc");

            var detail = new OrderDetails
            {
                OrderDetailId = "ODT" + IdGenerator.RandomDigits(),
                OrdersId = dto.OrdersId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                UnitPrice = dto.UnitPrice
            };

            _context.OrderDetails.Add(detail);
            _context.SaveChanges();

            return Ok("Chi tiết đơn hàng đã được thêm");
        }

        //Cập nhật chi tiết đơn hàng
        [HttpPut]
        public IActionResult UpdateOrderDetail([FromBody] OrderDetailUpdateDTO dto)
        {
            var detail = _context.OrderDetails.FirstOrDefault(d => d.OrderDetailId == dto.OrderDetailId);
            if (detail == null)
                return NotFound("Không tìm thấy chi tiết đơn hàng");

            detail.Quantity = dto.Quantity;
            detail.UnitPrice = dto.UnitPrice;

            _context.SaveChanges();
            return Ok("Chi tiết đơn hàng đã được cập nhật");
        }

        //Xóa chi tiết đơn hàng
        [HttpDelete("{id}")]
        public IActionResult DeleteOrderDetail(string id)
        {
            var detail = _context.OrderDetails.FirstOrDefault(d => d.OrderDetailId == id);
            if (detail == null)
                return NotFound("Không tìm thấy chi tiết đơn hàng để xóa");

            _context.OrderDetails.Remove(detail);
            _context.SaveChanges();

            return Ok("Chi tiết đơn hàng đã được xóa");
        }

        //Lấy chi tiết đơn hàng theo OrdersId
        [HttpGet("order/{ordersId}")]
        public IActionResult GetOrderDetailsByOrder(string ordersId)
        {
            var details = _context.OrderDetails
                .Where(d => d.OrdersId == ordersId)
                .Select(d => new OrderDetailResponseDTO
                {
                    OrderDetailId = d.OrderDetailId,
                    OrdersId = d.OrdersId ?? "",
                    ProductId = d.ProductId ?? "",
                    ProductName = "",
                    Quantity = d.Quantity ?? 0,
                    UnitPrice = d.UnitPrice ?? 0,
                    TotalPrice = (d.Quantity ?? 0) * (d.UnitPrice ?? 0)
                })
                .ToList();

            return Ok(details);
        }
    }
}

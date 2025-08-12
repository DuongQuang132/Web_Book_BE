using Microsoft.EntityFrameworkCore;
using Web_Book_BE.DTO;
using Web_Book_BE.Models;
using Web_Book_BE.Services.Interfaces;
using Web_Book_BE.Utils;

namespace Web_Book_BE.Services
{
    public class OrderService : IOrderService
    {
        private readonly BookStoreDbContext _context;

        public OrderService(BookStoreDbContext context)
        {
            _context = context;
        }

        public string CreateOrder(OrderCreateDTO dto)
        {
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
            return order.OrdersId;
        }

        public void UpdateOrder(OrderUpdateDTO dto)
        {
            var order = _context.Orders.FirstOrDefault(o => o.OrdersId == dto.OrdersId)
                        ?? throw new Exception("Không tìm thấy đơn hàng");

            order.TotalAmount = dto.TotalAmount;
            order.Status = dto.Status;
            order.ShippingAddress = dto.ShippingAddress;
            order.UpdatedAt = DateTime.UtcNow;

            _context.SaveChanges();
        }

        public void UpdateOrderStatus(OrderStatusUpdateDTO dto)
        {
            var order = _context.Orders.FirstOrDefault(o => o.OrdersId == dto.OrdersId)
                        ?? throw new Exception("Không tìm thấy đơn hàng");

            order.Status = dto.Status;
            order.UpdatedAt = DateTime.UtcNow;

            _context.SaveChanges();
        }

        public void DeleteOrder(string id)
        {
            var order = _context.Orders.FirstOrDefault(o => o.OrdersId == id)
                        ?? throw new Exception("Không tìm thấy đơn hàng");

            _context.Orders.Remove(order);
            _context.SaveChanges();
        }

        public OrderResponseDTO GetOrderById(string id)
        {
            var order = _context.Orders.Include(o => o.User).FirstOrDefault(o => o.OrdersId == id)
                        ?? throw new Exception("Không tìm thấy đơn hàng");

            return MapToDTO(order);
        }

        public List<OrderResponseDTO> GetOrdersByUser(string userId)
        {
            return _context.Orders
                .Include(o => o.User)
                .Where(o => o.UserId == userId)
                .Select(MapToDTO)
                .ToList();
        }

        public List<OrderResponseDTO> GetAllOrders()
        {
            return _context.Orders
                .Include(o => o.User)
                .Select(MapToDTO)
                .ToList();
        }

        private static OrderResponseDTO MapToDTO(Orders order) => new OrderResponseDTO
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
    }
}

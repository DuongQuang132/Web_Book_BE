using Web_Book_BE.DTO;
using Web_Book_BE.Models;
using Web_Book_BE.Utils;
using Microsoft.EntityFrameworkCore;
using Web_Book_BE.Services.Interfaces;


namespace Web_Book_BE.Services
{
    public class OrderDetailsService : IOrderDetailsService
    {
        private readonly BookStoreDbContext _context;

        public OrderDetailsService(BookStoreDbContext context)
        {
            _context = context;
        }

        public string CreateOrderDetail(OrderDetailCreateDTO dto)
        {
            if (string.IsNullOrEmpty(dto.OrdersId) || string.IsNullOrEmpty(dto.ProductId))
                throw new ArgumentException("OrdersId và ProductId là bắt buộc");

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

            return detail.OrderDetailId;
        }

        public bool UpdateOrderDetail(OrderDetailUpdateDTO dto)
        {
            var detail = _context.OrderDetails.FirstOrDefault(d => d.OrderDetailId == dto.OrderDetailId);
            if (detail == null) return false;

            detail.Quantity = dto.Quantity;
            detail.UnitPrice = dto.UnitPrice;
            _context.SaveChanges();

            return true;
        }

        public bool DeleteOrderDetail(string id)
        {
            var detail = _context.OrderDetails.FirstOrDefault(d => d.OrderDetailId == id);
            if (detail == null) return false;

            _context.OrderDetails.Remove(detail);
            _context.SaveChanges();

            return true;
        }

        public List<OrderDetailResponseDTO> GetOrderDetailsByOrder(string ordersId)
        {
            return _context.OrderDetails
                .Where(d => d.OrdersId == ordersId)
                .Select(d => new OrderDetailResponseDTO
                {
                    OrderDetailId = d.OrderDetailId,
                    OrdersId = d.OrdersId ?? "",
                    ProductId = d.ProductId ?? "",
                    ProductName = "", // Có thể bổ sung sau
                    Quantity = d.Quantity ?? 0,
                    UnitPrice = d.UnitPrice ?? 0,
                    TotalPrice = (d.Quantity ?? 0) * (d.UnitPrice ?? 0)
                })
                .ToList();
        }
    }
}

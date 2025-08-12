using Web_Book_BE.DTO;

namespace Web_Book_BE.Services.Interfaces
{
    public interface IOrderService
    {
        string CreateOrder(OrderCreateDTO dto);
        void UpdateOrder(OrderUpdateDTO dto);
        void UpdateOrderStatus(OrderStatusUpdateDTO dto);
        void DeleteOrder(string id);
        OrderResponseDTO GetOrderById(string id);
        List<OrderResponseDTO> GetOrdersByUser(string userId);
        List<OrderResponseDTO> GetAllOrders();
    }
}

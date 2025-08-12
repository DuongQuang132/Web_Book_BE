using Web_Book_BE.DTO;

namespace Web_Book_BE.Services.Interfaces
{
    public interface IOrderDetailsService
    {
        string CreateOrderDetail(OrderDetailCreateDTO dto);
        bool UpdateOrderDetail(OrderDetailUpdateDTO dto);
        bool DeleteOrderDetail(string id);
        List<OrderDetailResponseDTO> GetOrderDetailsByOrder(string ordersId);
    }
}

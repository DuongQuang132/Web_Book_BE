using Web_Book_BE.DTO;

namespace Web_Book_BE.Services.Interfaces
{
    public interface IPaymentService
    {
        string CreatePayment(PaymentCreateDTO dto);
        string UpdatePayment(PaymentUpdateDTO dto);
        string UpdatePaymentStatus(PaymentStatusUpdateDTO dto);
        string DeletePayment(string id);
        List<PaymentResponseDTO> GetPaymentsByOrder(string ordersId);
        List<PaymentResponseDTO> GetAllPayments();
    }
}

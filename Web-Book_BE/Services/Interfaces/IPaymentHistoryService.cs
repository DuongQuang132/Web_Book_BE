using Web_Book_BE.DTO;

namespace Web_Book_BE.Services.Interfaces
{
    public interface IPaymentHistoryService
    {
        string CreatePaymentHistory(PaymentHistoryCreateDTO dto);
        List<PaymentHistoryResponseDTO> GetHistoryByPayment(string paymentId);
        List<PaymentHistoryResponseDTO> FilterHistoryByDate(PaymentHistoryFilterDTO dto);
        List<PaymentHistoryResponseDTO> GetAllPaymentHistories();
    }
}

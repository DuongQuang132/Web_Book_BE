namespace Web_Book_BE.Services.Interfaces
{
    public interface IMomoService
    {
        Task<string> CreatePaymentAsync(decimal amount);
    }
}

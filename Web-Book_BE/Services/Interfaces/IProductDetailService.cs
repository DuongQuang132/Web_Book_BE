using Web_Book_BE.DTO;

namespace Web_Book_BE.Services.Interfaces
{
    public interface IProductDetailService
    {
        Task<string> CreateProductDetailAsync(ProductDetailCreateDTO dto);
        Task<string> UpdateProductDetailAsync(ProductDetailUpdateDTO dto);
        Task<ProductDetailByProductDTO?> GetDetailByProductAsync(string productId);
        Task<List<ProductDetailResponseDTO>> GetAllProductDetailsAsync();
    }
}


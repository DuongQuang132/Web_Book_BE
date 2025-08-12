using Web_Book_BE.DTO;

namespace Web_Book_BE.Services.Interfaces
{
    public interface IProductService
    {
        Task<string> CreateProductAsync(ProductCreateDTO dto);
        Task<string> UpdateProductAsync(ProductUpdateDTO dto);
        Task<string> IDeleteProductAsync(ProductDeleteDTO dto);
        Task<ProductResponseDTO?> GetProductByIdAsync(string id);
        Task<List<ProductResponseDTO>> GetAllProductsAsync();
        Task<List<ProductResponseDTO>> FilterProductsAsync(ProductFilterDTO dto);
    }
}


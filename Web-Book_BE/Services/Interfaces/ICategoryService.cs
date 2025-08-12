using System.Threading.Tasks;
using Web_Book_BE.DTO;

namespace Web_Book_BE.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<string> CreateCategoryAsync(CategoryCreateDTO dto);
        Task<string> UpdateCategoryAsync(CategoryUpdateDTO dto);
        Task<string> DeleteCategoryAsync(string id);
        Task<CategoryResponseDTO?> GetCategoryByIdAsync(string id);
        Task<List<CategoryResponseDTO>> GetAllCategoriesAsync();
    }
}


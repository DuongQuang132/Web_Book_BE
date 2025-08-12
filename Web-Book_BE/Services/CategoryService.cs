using Microsoft.EntityFrameworkCore;
using Web_Book_BE.DTO;
using Web_Book_BE.Models;
using Web_Book_BE.Services.Interfaces;
using Web_Book_BE.Utils;

public class CategoryService : ICategoryService
{
    private readonly BookStoreDbContext _context;

    public CategoryService(BookStoreDbContext context)
    {
        _context = context;
    }

    public async Task<string> CreateCategoryAsync(CategoryCreateDTO dto)
    {
        if (string.IsNullOrWhiteSpace(dto.CategoriesName))
            return "Tên danh mục không được để trống";

        var category = new Categories
        {
            CategoriesId = "CAT" + IdGenerator.RandomDigits(),
            CategoriesName = dto.CategoriesName,
            Description = dto.Description
        };

        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();

        return "Danh mục đã được tạo thành công";
    }
    public async Task<string> UpdateCategoryAsync(CategoryUpdateDTO dto)
    {
        var category = await _context.Categories
            .FirstOrDefaultAsync(c => c.CategoriesId == dto.CategoriesId);

        if (category == null)
            return "Không tìm thấy danh mục";

        category.CategoriesName = dto.CategoriesName;
        category.Description = dto.Description;

        await _context.SaveChangesAsync();
        return "Danh mục đã được cập nhật";
    }
    public async Task<string> DeleteCategoryAsync(string id)
    {
        var category = await _context.Categories
            .FirstOrDefaultAsync(c => c.CategoriesId == id);

        if (category == null)
            return "Không tìm thấy danh mục để xóa";

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        return "Danh mục đã được xóa";
    }
    public async Task<CategoryResponseDTO?> GetCategoryByNameAsync(string name)
    {
        var category = await _context.Categories
            .FirstOrDefaultAsync(c => c.CategoriesName.ToLower() == name.ToLower());

        if (category == null)
            return null;

        return new CategoryResponseDTO
        {
            CategoriesId = category.CategoriesId,
            CategoriesName = category.CategoriesName,
            Description = category.Description
        };
    }
    public async Task<List<CategoryResponseDTO>> GetAllCategoriesAsync()
    {
        return await _context.Categories
            .Select(c => new CategoryResponseDTO
            {
                CategoriesId = c.CategoriesId,
                CategoriesName = c.CategoriesName,
                Description = c.Description
            })
            .ToListAsync();
    }
}
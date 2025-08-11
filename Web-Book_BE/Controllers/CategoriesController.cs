using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Book_BE.DTO;
using Web_Book_BE.Models;
using Web_Book_BE.Utils;

namespace Web_Book_BE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly BookStoreDbContext _context;

        public CategoriesController(BookStoreDbContext context)
        {
            _context = context;
        }

        // ✅ Tạo mới danh mục
        [HttpPost]
        public IActionResult CreateCategory([FromBody] CategoryCreateDTO dto)
        {
            if (string.IsNullOrEmpty(dto.CategoriesName))
                return BadRequest("Tên danh mục không được để trống");

            var category = new Categories
            {
                CategoriesId = "CAT" + IdGenerator.RandomDigits(),
                CategoriesName = dto.CategoriesName,
                Description = dto.Description
            };

            _context.Categories.Add(category);
            _context.SaveChanges();

            return Ok("Danh mục đã được tạo thành công");
        }

        // ✅ Cập nhật danh mục
        [HttpPut]
        public IActionResult UpdateCategory([FromBody] CategoryUpdateDTO dto)
        {
            var category = _context.Categories.FirstOrDefault(c => c.CategoriesId == dto.CategoriesId);
            if (category == null)
                return NotFound("Không tìm thấy danh mục");

            category.CategoriesName = dto.CategoriesName;
            category.Description = dto.Description;

            _context.SaveChanges();
            return Ok("Danh mục đã được cập nhật");
        }

        // ✅ Xóa danh mục
        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(string id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.CategoriesId == id);
            if (category == null)
                return NotFound("Không tìm thấy danh mục để xóa");

            _context.Categories.Remove(category);
            _context.SaveChanges();

            return Ok("Danh mục đã được xóa");
        }

        // ✅ Lấy danh mục theo ID
        [HttpGet("{id}")]
        public IActionResult GetCategoryById(string id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.CategoriesId == id);
            if (category == null)
                return NotFound("Không tìm thấy danh mục");

            var response = new CategoryResponseDTO
            {
                CategoriesId = category.CategoriesId,
                CategoriesName = category.CategoriesName,
                Description = category.Description
            };

            return Ok(response);
        }

        // ✅ Lấy tất cả danh mục
        [HttpGet]
        public IActionResult GetAllCategories()
        {
            var categories = _context.Categories
                .Select(c => new CategoryResponseDTO
                {
                    CategoriesId = c.CategoriesId,
                    CategoriesName = c.CategoriesName,
                    Description = c.Description
                })
                .ToList();

            return Ok(categories);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Book_BE.DTO;
using Web_Book_BE.Models;
using Web_Book_BE.Services;
using Web_Book_BE.Services.Interfaces;
using Web_Book_BE.Utils;

namespace Web_Book_BE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        //Tạo mới danh mục
        [HttpPost("create")]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryCreateDTO dto)
        {
            var message = await _categoryService.CreateCategoryAsync(dto);

            return message == "Danh mục đã được tạo thành công"
                ? Ok(message)
                : BadRequest(message);
        }

        //Cập nhật danh mục
        [HttpPut("update")]
        public async Task<IActionResult> UpdateCategory([FromBody] CategoryUpdateDTO dto)
        {
            var message = await _categoryService.UpdateCategoryAsync(dto);

            return message == "Danh mục đã được cập nhật"
                ? Ok(message)
                : NotFound(message);
        }

        //Xóa danh mục
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            var message = await _categoryService.DeleteCategoryAsync(id);

            return message == "Danh mục đã được xóa"
                ? Ok(message)
                : NotFound(message);
        }

        //Lấy danh mục theo tên
        [HttpGet("CategoryName")]
        public async Task<IActionResult> GetCategoryByName([FromQuery] string name)
        {
            var category = await _categoryService.GetCategoryByNameAsync(name);

            return category != null
                ? Ok(category)
                : NotFound("Không tìm thấy danh mục");
        }

        //Lấy tất cả danh mục
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }
    }
}

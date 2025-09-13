using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Book_BE.DTO;
using Web_Book_BE.Models;
using Web_Book_BE.Services.Interfaces;
using Web_Book_BE.Utils;

namespace Web_Book_BE.Controllers
{
    [ApiController]
    [Route("product")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateProduct([FromForm] ProductCreateDTO dto)
        {
            try
            {
                var result = await _productService.CreateProductAsync(dto);

                if (result == "Tạo sản phẩm không thành công")
                    return StatusCode(500, result);

                return Ok(new { Message = "Tạo sản phẩm thành công", ProductId = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }

        [HttpPut("update-product")]
        public async Task<IActionResult> UpdateProduct([FromForm] ProductUpdateDTO dto)
        {
            try
            {
                var result = await _productService.UpdateProductAsync(dto, dto.Image);
                if (string.IsNullOrEmpty(result))
                    return BadRequest("Cập nhật sản phẩm thất bại");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteProduct([FromBody] ProductDeleteDTO dto)
        {
            var result = await _productService.IDeleteProductAsync(dto);
            return Ok(new { Message = result });
        }

        //Xóa mềm sản phẩm
        [HttpPatch("delete")]
        public async Task<IActionResult> IDeleteProduct([FromBody] ProductDeleteDTO dto)
        {
            var message = await _productService.IDeleteProductAsync(dto);

            if (message == "Không tìm thấy sản phẩm")
                return NotFound(message);

            if (message == "Xóa mềm thất bại")
                return StatusCode(500, message);

            return Ok(message);
        }

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetProductByCategoryId(string categoryId)
        {
            var product = await _productService.GetProductByCategoryId(categoryId);

            return Ok(product);
        }

        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetProductById(string id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            return product != null
                ? Ok(product)
                : NotFound("Không tìm thấy sản phẩm");
        }

        [HttpGet("search")]
        public async Task<IActionResult> GetProductsByName([FromQuery] string keyword)
        {
            var products = await _productService.GetProductsByNameAsync(keyword);
            return Ok(products);
        }


        //Lấy tất cả sản phẩm
        [HttpGet("getall")]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        //Tìm kiếm và lọc sản phẩm
        [HttpPost("filter")]
        public async Task<IActionResult> FilterProducts([FromBody] ProductFilterDTO dto)
        {
            var products = await _productService.FilterProductsAsync(dto);
            return Ok(products);
        }
    }
}

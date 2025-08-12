using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Book_BE.DTO;
using Web_Book_BE.Models;
using Web_Book_BE.Services.Interfaces;
using Web_Book_BE.Utils;

namespace Web_Book_BE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        //Tạo sản phẩm
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreateDTO dto)
        {
            var productId = await _productService.CreateProductAsync(dto);

            if (string.IsNullOrEmpty(productId))
                return StatusCode(500, "Tạo sản phẩm thất bại");

            return Ok(new { Message = "Tạo sản phẩm thành công", ProductId = productId });
        }

        //Cập nhật sản phẩm
        [HttpPut]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductUpdateDTO dto)
        {
            var message = await _productService.UpdateProductAsync(dto);

            if (message == "Không tìm thấy sản phẩm")
                return NotFound(message);

            if (message == "Cập nhật thất bại")
                return StatusCode(500, message);

            return Ok(message);
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

        //Lấy sản phẩm theo ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(string id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            return product != null
                ? Ok(product)
                : NotFound("Không tìm thấy sản phẩm");
        }

        //Lấy tất cả sản phẩm
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        // ✅ Tìm kiếm và lọc sản phẩm
        [HttpPost("filter")]
        public async Task<IActionResult> FilterProducts([FromBody] ProductFilterDTO dto)
        {
            var products = await _productService.FilterProductsAsync(dto);
            return Ok(products);
        }
    }
}

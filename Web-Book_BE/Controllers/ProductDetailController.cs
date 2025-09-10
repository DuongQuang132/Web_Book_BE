using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Book_BE.DTO;
using Web_Book_BE.Models;
using Web_Book_BE.Services.Interfaces;
using Web_Book_BE.Utils;

namespace Web_Book_BE.Controllers
{
    [ApiController]
    [Route("productdetail")]
    public class ProductDetailController : ControllerBase
    {
        private readonly IProductDetailService _productDetailService;
        public ProductDetailController (IProductDetailService productDetailService)
        {
            _productDetailService = productDetailService;
        }

        //Tạo thông tin chi tiết sản phẩm
        [HttpPost]
        public async Task<IActionResult> CreateProductDetail([FromBody] ProductDetailCreateDTO dto)
        {
            var message = await _productDetailService.CreateProductDetailAsync(dto);

            return message == "Thông tin chi tiết sản phẩm đã được tạo"
                ? Ok(message)
                : BadRequest(message);
        }

        //Cập nhật thông tin chi tiết sản phẩm
        [HttpPut]
        public async Task<IActionResult> UpdateProductDetail([FromBody] ProductDetailUpdateDTO dto)
        {
            var message = await _productDetailService.UpdateProductDetailAsync(dto);

            return message == "Thông tin chi tiết sản phẩm đã được cập nhật"
                ? Ok(message)
                : NotFound(message);
        }

        //Lấy chi tiết theo ProductId
        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetDetailByProduct(string productId)
        {
            var response = await _productDetailService.GetDetailByProductAsync(productId);

            return response != null 
                ? Ok(response)
                : NotFound("Không tìm thấy thông tin chi tiết sản phẩm");
        }

        //Lấy tất cả chi tiết sản phẩm
        [HttpGet]
        public async Task<IActionResult> GetAllProductDetails()
        {
            var details = await _productDetailService.GetAllProductDetailsAsync();
            return Ok(details);
        }
    }
}

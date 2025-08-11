using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Book_BE.DTO;
using Web_Book_BE.Models;
using Web_Book_BE.Utils;

namespace Web_Book_BE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductDetailsController : ControllerBase
    {
        private readonly BookStoreDbContext _context;

        public ProductDetailsController(BookStoreDbContext context)
        {
            _context = context;
        }

        //Tạo thông tin chi tiết sản phẩm
        [HttpPost]
        public IActionResult CreateProductDetail([FromBody] ProductDetailCreateDTO dto)
        {
            if (string.IsNullOrEmpty(dto.ProductId))
                return BadRequest("ProductId là bắt buộc");

            var detail = new ProductDetails
            {
                ProductDetailId = "PDT" + IdGenerator.RandomDigits(),
                ProductId = dto.ProductId,
                Publisher = dto.Publisher,
                PublishDate = dto.PublishDate.HasValue
                    ? DateOnly.FromDateTime(dto.PublishDate.Value)
                    : null,
                PageCount = dto.PageCount,
                Language = dto.Language
            };

            _context.ProductDetails.Add(detail);
            _context.SaveChanges();

            return Ok("Thông tin chi tiết sản phẩm đã được tạo");
        }

        //Cập nhật thông tin chi tiết sản phẩm
        [HttpPut]
        public IActionResult UpdateProductDetail([FromBody] ProductDetailUpdateDTO dto)
        {
            var detail = _context.ProductDetails.FirstOrDefault(d => d.ProductDetailId == dto.ProductDetailId);
            if (detail == null)
                return NotFound("Không tìm thấy thông tin chi tiết sản phẩm");

            detail.Publisher = dto.Publisher;
            detail.PublishDate = dto.PublishDate.HasValue
                ? DateOnly.FromDateTime(dto.PublishDate.Value)
                : null;
            detail.PageCount = dto.PageCount;
            detail.Language = dto.Language;

            _context.SaveChanges();
            return Ok("Thông tin chi tiết sản phẩm đã được cập nhật");
        }

        //Lấy chi tiết theo ProductId
        [HttpGet("product/{productId}")]
        public IActionResult GetDetailByProduct(string productId)
        {
            var detail = _context.ProductDetails
                .FirstOrDefault(d => d.ProductId == productId);

            if (detail == null)
                return NotFound("Không tìm thấy thông tin chi tiết sản phẩm");

            var response = new ProductDetailResponseDTO
            {
                ProductDetailId = detail.ProductDetailId,
                ProductId = detail.ProductId ?? "",
                Publisher = detail.Publisher ?? "",
                PublishDate = detail.PublishDate.HasValue
                    ? detail.PublishDate.Value.ToDateTime(TimeOnly.MinValue)
                    : null,
                PageCount = detail.PageCount,
                Language = detail.Language ?? ""
            };

            return Ok(response);
        }

        //Lấy tất cả chi tiết sản phẩm
        [HttpGet]
        public IActionResult GetAllProductDetails()
        {
            var details = _context.ProductDetails
                .Select(detail => new ProductDetailResponseDTO
                {
                    ProductDetailId = detail.ProductDetailId,
                    ProductId = detail.ProductId ?? "",
                    Publisher = detail.Publisher ?? "",
                    PublishDate = detail.PublishDate.HasValue
                        ? detail.PublishDate.Value.ToDateTime(TimeOnly.MinValue)
                        : null,
                    PageCount = detail.PageCount,
                    Language = detail.Language ?? ""
                })
                .ToList();

            return Ok(details);
        }
    }
}

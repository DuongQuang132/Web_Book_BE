using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Book_BE.DTO;
using Web_Book_BE.Models;
using Web_Book_BE.Utils;

namespace Web_Book_BE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly BookStoreDbContext _context;

        public ProductController(BookStoreDbContext context)
        {
            _context = context;
        }

        //Tạo sản phẩm
        [HttpPost]
        public IActionResult CreateProduct([FromBody] ProductCreateDTO dto)
        {
            var product = new Product
            {
                ProductId = "PRO" + IdGenerator.RandomDigits(),
                ProductName = dto.ProductName,
                AuthorId = dto.AuthorId,
                CategoriesId = dto.CategoriesId,
                Price = dto.Price,
                Discount = dto.Discount,
                Description = dto.Description,
                Quantity = dto.Quantity,
                ImageUrl = dto.ImageUrl,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Products.Add(product);
            _context.SaveChanges();

            return Ok("Sản phẩm đã được tạo thành công");
        }

        //Cập nhật sản phẩm
        [HttpPut]
        public IActionResult UpdateProduct([FromBody] ProductUpdateDTO dto)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductId == dto.ProductId);
            if (product == null)
                return NotFound("Không tìm thấy sản phẩm");

            product.ProductName = dto.ProductName;
            product.AuthorId = dto.AuthorId;
            product.CategoriesId = dto.CategoriesId;
            product.Price = dto.Price;
            product.Discount = dto.Discount;
            product.Description = dto.Description;
            product.Quantity = dto.Quantity;
            product.ImageUrl = dto.ImageUrl;
            product.UpdatedAt = DateTime.UtcNow;

            _context.SaveChanges();
            return Ok("Sản phẩm đã được cập nhật");
        }

        //Xóa mềm sản phẩm
        [HttpPatch("delete")]
        public IActionResult SoftDeleteProduct([FromBody] ProductDeleteDTO dto)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductId == dto.ProductId);
            if (product == null)
                return NotFound("Không tìm thấy sản phẩm");

            product.IsDeleted = true;
            product.UpdatedAt = DateTime.UtcNow;

            _context.SaveChanges();
            return Ok("Sản phẩm đã được xóa mềm");
        }

        //Lấy sản phẩm theo ID
        [HttpGet("{id}")]
        public IActionResult GetProductById(string id)
        {
            var product = _context.Products
                .Include(p => p.Author)
                .Include(p => p.Categories)
                .FirstOrDefault(p => p.ProductId == id && p.IsDeleted != true);

            if (product == null)
                return NotFound("Không tìm thấy sản phẩm");

            var response = new ProductResponseDTO
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName ?? "",
                AuthorName = product.Author?.AuthorName ?? "",
                CategoriesName = product.Categories?.CategoriesName ?? "",
                Price = product.Price,
                Discount = product.Discount ?? "",
                Description = product.Description ?? "",
                Quantity = product.Quantity,
                ImageUrl = product.ImageUrl ?? "",
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt ?? DateTime.MinValue
            };

            return Ok(response);
        }

        //Lấy tất cả sản phẩm
        [HttpGet]
        public IActionResult GetAllProducts()
        {
            var products = _context.Products
                .Include(p => p.Author)
                .Include(p => p.Categories)
                .Where(p => p.IsDeleted != true)
                .Select(product => new ProductResponseDTO
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName ?? "",
                    AuthorName = product.Author.AuthorName ?? "",
                    CategoriesName = product.Categories.CategoriesName ?? "",
                    Price = product.Price,
                    Discount = product.Discount ?? "",
                    Description = product.Description ?? "",
                    Quantity = product.Quantity,
                    ImageUrl = product.ImageUrl ?? "",
                    CreatedAt = product.CreatedAt,
                    UpdatedAt = product.UpdatedAt ?? DateTime.MinValue
                })
                .ToList();

            return Ok(products);
        }

        // ✅ Tìm kiếm và lọc sản phẩm
        [HttpPost("filter")]
        public IActionResult FilterProducts([FromBody] ProductFilterDTO dto)
        {
            var query = _context.Products
                .Include(p => p.Author)
                .Include(p => p.Categories)
                .Where(p => p.IsDeleted != true);

            if (!string.IsNullOrEmpty(dto.Keyword))
            {
                query = query.Where(p =>
                    (p.ProductName ?? "").Contains(dto.Keyword) ||
                    (p.Description ?? "").Contains(dto.Keyword));
            }

            if (!string.IsNullOrEmpty(dto.AuthorId))
                query = query.Where(p => p.AuthorId == dto.AuthorId);

            if (!string.IsNullOrEmpty(dto.CategoriesId))
                query = query.Where(p => p.CategoriesId == dto.CategoriesId);

            if (dto.MinPrice.HasValue)
                query = query.Where(p => p.Price >= dto.MinPrice.Value);

            if (dto.MaxPrice.HasValue)
                query = query.Where(p => p.Price <= dto.MaxPrice.Value);

            if (dto.OnlyDiscounted == true)
                query = query.Where(p => !string.IsNullOrEmpty(p.Discount) && p.Discount != "0");

            if (dto.OnlyAvailable == true)
                query = query.Where(p => p.Quantity > 0);

            var totalItems = query.Count();

            var products = query
                .Skip((dto.PageIndex - 1) * dto.PageSize)
                .Take(dto.PageSize)
                .Select(product => new ProductResponseDTO
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName ?? "",
                    AuthorName = product.Author.AuthorName ?? "",
                    CategoriesName = product.Categories.CategoriesName ?? "",
                    Price = product.Price,
                    Discount = product.Discount ?? "",
                    Description = product.Description ?? "",
                    Quantity = product.Quantity,
                    ImageUrl = product.ImageUrl ?? "",
                    CreatedAt = product.CreatedAt,
                    UpdatedAt = product.UpdatedAt ?? DateTime.MinValue
                })
                .ToList();

            return Ok(new
            {
                TotalItems = totalItems,
                PageIndex = dto.PageIndex,
                PageSize = dto.PageSize,
                Items = products
            });
        }
    }
}

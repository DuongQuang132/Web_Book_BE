using Microsoft.EntityFrameworkCore;
using Web_Book_BE.DTO;
using Web_Book_BE.Models;
using Web_Book_BE.Services.Interfaces;
using Web_Book_BE.Utils;

namespace Web_Book_BE.Services
{
    public class ProductService : IProductService
    {
        private readonly BookStoreDbContext _context;

        public ProductService(BookStoreDbContext context)
        {
            _context = context;
        }
        public async Task<string> CreateProductAsync(ProductCreateDTO dto)
        {
            var productId = "PRO" + IdGenerator.RandomDigits();

            var product = new Product
            {
                ProductId = productId,
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
            var result = await _context.SaveChangesAsync();

            return result > 0 ? productId : string.Empty;
        }
        public async Task<string> UpdateProductAsync(ProductUpdateDTO dto)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductId == dto.ProductId);

            if (product == null)
                return "Không tìm thấy sản phẩm";

            product.ProductName = dto.ProductName;
            product.AuthorId = dto.AuthorId;
            product.CategoriesId = dto.CategoriesId;
            product.Price = dto.Price;
            product.Discount = dto.Discount;
            product.Description = dto.Description;
            product.Quantity = dto.Quantity;
            product.ImageUrl = dto.ImageUrl;
            product.UpdatedAt = DateTime.UtcNow;

            var result = await _context.SaveChangesAsync();
            return result > 0 ? "Sản phẩm đã được cập nhật" : "Cập nhật thất bại";
        }
        public async Task<string> IDeleteProductAsync(ProductDeleteDTO dto)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductId == dto.ProductId);

            if (product == null)
                return "Không tìm thấy sản phẩm";

            product.IsDeleted = true;
            product.UpdatedAt = DateTime.UtcNow;

            var result = await _context.SaveChangesAsync();
            return result > 0 ? "Sản phẩm đã được xóa mềm" : "Xóa mềm thất bại";
        }
        public async Task<ProductResponseDTO?> GetProductByIdAsync(string id)
        {
            var product = await _context.Products
                .Include(p => p.Author)
                .Include(p => p.Categories)
                .FirstOrDefaultAsync(p => p.ProductId == id && p.IsDeleted != true);

            if (product == null)
                return null;

            return new ProductResponseDTO
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
        }
        public async Task<List<ProductResponseDTO>> GetAllProductsAsync()
        {
            return await _context.Products
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
                .ToListAsync();
        }
        public async Task<List<ProductResponseDTO>> FilterProductsAsync(ProductFilterDTO dto)
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

            var products = await query
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
                .ToListAsync();

            return products;
        }

    }
}

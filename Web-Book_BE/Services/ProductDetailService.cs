using CloudinaryDotNet.Actions;
using Microsoft.EntityFrameworkCore;
using Web_Book_BE.DTO;
using Web_Book_BE.Models;
using Web_Book_BE.Services.Interfaces;
using Web_Book_BE.Utils;

namespace Web_Book_BE.Services
{
    public class ProductDetailService : IProductDetailService
    {
        private readonly BookStoreDbContext _context;

        public ProductDetailService(BookStoreDbContext context)
        {
            _context = context;
        }

        public async Task<string> CreateProductDetailAsync(ProductDetailCreateDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.ProductId))
                return "ProductId là bắt buộc";

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

            await _context.ProductDetails.AddAsync(detail);
            var result = await _context.SaveChangesAsync();

            return result > 0
                ? "Thông tin chi tiết sản phẩm đã được tạo"
                : "Tạo thông tin chi tiết thất bại";
        }
        public async Task<string> UpdateProductDetailAsync(ProductDetailUpdateDTO dto)
        {
            var detail = await _context.ProductDetails
                .FirstOrDefaultAsync(d => d.ProductDetailId == dto.ProductDetailId);

            if (detail == null)
                return "Không tìm thấy thông tin chi tiết sản phẩm";

            detail.Publisher = dto.Publisher;
            detail.PublishDate = dto.PublishDate.HasValue
                ? DateOnly.FromDateTime(dto.PublishDate.Value)
                : null;
            detail.PageCount = dto.PageCount;
            detail.Language = dto.Language;

            var result = await _context.SaveChangesAsync();

            return result > 0
                ? "Thông tin chi tiết sản phẩm đã được cập nhật"
                : "Cập nhật thất bại";
        }
        public async Task<ProductDetailByProductDTO> GetDetailByProductAsync(string productId)
        {
            var productDetail = await _context.ProductDetails
                .Include(d => d.Product)
                .FirstOrDefaultAsync(d => d.ProductId == productId);

            if (productDetail == null)
                return null;

            return new ProductDetailByProductDTO
            {
                  ProductId =productDetail.ProductId,
                ProductDetailId = productDetail.ProductDetailId,
                 Name = productDetail.Product.Name,
                 AuthorName = productDetail.Product.Author.AuthorName,
                 CategoriesName = productDetail.Product.Categories.Name,
                 Price = productDetail.Product.Price,
                 Discount = productDetail.Product.Discount,
                 Description = productDetail.Product.Description,
                 ImageUrl = productDetail.Product.ImageUrl,
            };
        }
        public async Task<List<ProductDetailResponseDTO>> GetAllProductDetailsAsync()
        {
            return await _context.ProductDetails
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
                .ToListAsync();
        }

    }
}

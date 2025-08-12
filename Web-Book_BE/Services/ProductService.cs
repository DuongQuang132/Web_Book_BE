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
        private readonly CloudinaryUtil _cloudinaryHelper;
        private readonly IImageService _imageService;

        public ProductService(BookStoreDbContext context, CloudinaryUtil cloudinaryHelper, IImageService imageService)
        {
            _context = context;
            _cloudinaryHelper = cloudinaryHelper;
            _imageService = imageService;
        }

        public async Task<string> CreateProductAsync(ProductCreateDTO dto)
        {
            // Tìm AuthorId từ tên tác giả
            var author = await _context.Authors
                .FirstOrDefaultAsync(a => a.AuthorName == dto.AuthorName);
            if (author == null)
                return "Tạo sản phẩm không thành công";

            // Tìm CategoryId từ tên category
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoriesName == dto.CategoriesName);
            if (category == null)
                return "Tạo sản phẩm không thành công";

            var productId = "PRO" + IdGenerator.RandomDigits();
            string imageUrl = string.Empty;

            // Xử lý ảnh
            if (dto.Image != null && _imageService.IsValidImageFile(dto.Image))
            {
                imageUrl = await _imageService.UploadImageFromFileAsync(dto.Image);
            }
            else if (!string.IsNullOrEmpty(dto.ImageUrl) && _imageService.IsValidImageUrl(dto.ImageUrl))
            {
                imageUrl = await _imageService.UploadImageFromUrlAsync(dto.ImageUrl);
            }
            else if (dto.Image != null)
            {
                var uploadResult = await _cloudinaryHelper.UploadImageAsync(dto.Image);
                imageUrl = uploadResult.SecureUrl.ToString();
            }

            var product = new Product
            {
                ProductId = productId,
                ProductName = dto.ProductName,
                AuthorId = author.AuthorId,
                CategoriesId = category.CategoriesId,
                Price = dto.Price,
                Discount = dto.Discount,
                Description = dto.Description,
                Quantity = dto.Quantity,
                ImageUrl = imageUrl,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Products.Add(product);
            var result = await _context.SaveChangesAsync();

            return result > 0 ? productId : "Tạo sản phẩm không thành công";
        }

        public async Task<string> UpdateProductAsync(ProductUpdateDTO dto, IFormFile imageFile)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductId == dto.ProductId && p.IsDeleted == false);

            if (product == null)
                return "Sản phẩm không tồn tại!";

            // Tìm AuthorId từ tên
            var author = await _context.Authors
                .FirstOrDefaultAsync(a => a.AuthorName == dto.AuthorName);
            if (author == null)
                return "Author không tồn tại!";

            // Tìm CategoryId từ tên
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoriesName == dto.CategoriesName);
            if (category == null)
                return "Category không tồn tại!";

            product.ProductName = dto.ProductName;
            product.AuthorId = author.AuthorId;
            product.CategoriesId = category.CategoriesId;
            product.Price = dto.Price;
            product.Discount = dto.Discount;
            product.Description = dto.Description;
            product.Quantity = dto.Quantity;
            product.UpdatedAt = DateTime.UtcNow;

            // Xử lý ảnh
            if (dto.Image != null && _imageService.IsValidImageFile(dto.Image))
            {
                // Upload file lên Cloudinary
                product.ImageUrl = await _imageService.UploadImageFromFileAsync(dto.Image);
            }
            else if (!string.IsNullOrEmpty(dto.ImageUrl) && _imageService.IsValidImageUrl(dto.ImageUrl))
            {
                // Tải ảnh từ URL và upload lên Cloudinary
                product.ImageUrl = await _imageService.UploadImageFromUrlAsync(dto.ImageUrl);
            }
            else if (imageFile != null)
            {
                // Fallback: sử dụng CloudinaryUtil
                var uploadResult = await _cloudinaryHelper.UploadImageAsync(imageFile);
                product.ImageUrl = uploadResult.SecureUrl.ToString();
            }

            var result = await _context.SaveChangesAsync();
            return result > 0 ? "Cập nhật thành công" : "Cập nhật thất bại";
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

            if (product == null) return null;

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

            return await query
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

        public async Task<List<ProductResponseDTO>> GetProductsByNameAsync(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return new List<ProductResponseDTO>();

            return await _context.Products
                .Include(p => p.Author)
                .Include(p => p.Categories)
                .Where(p => p.IsDeleted != true &&
                            (p.ProductName ?? "").Contains(keyword))
                .Select(product => new ProductResponseDTO
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName ?? "",
                    AuthorName = product.Author != null ? product.Author.AuthorName : "",
                    CategoriesName = product.Categories != null ? product.Categories.CategoriesName : "",
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
    }
}

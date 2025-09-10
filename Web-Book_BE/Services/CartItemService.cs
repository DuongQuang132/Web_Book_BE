using Microsoft.EntityFrameworkCore;
using System;
using Web_Book_BE.DTO;
using Web_Book_BE.Models;
using Web_Book_BE.Services.Interfaces;
using Web_Book_BE.Utils;

namespace Web_Book_BE.Services
{
    public class CartItemService : ICartItemService
    {
        private readonly BookStoreDbContext _context;

        public CartItemService(BookStoreDbContext context)
        {
            _context = context;
        }

      

        public async Task<bool> AddToCartAsync(CartItemCreateDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.UserId) || string.IsNullOrWhiteSpace(dto.ProductId) || dto.Quantity <= 0)
                return false;

            var existingItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.UserId == dto.UserId && c.ProductId == dto.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity = (existingItem.Quantity ?? 0) + dto.Quantity;
            }
            else
            {
                var newItem = new CartItems
                {
                    CartItemId = "CI" + IdGenerator.RandomDigits(),
                    UserId = dto.UserId,
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity
                };
                await _context.CartItems.AddAsync(newItem);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<CartItemResponseDTO>> GetCartByUserAsync(string userId)
        {
            var cartItems = await _context.CartItems
                .Where(ci => ci.UserId == userId)
                .Include(ci => ci.Product)
                .Select(ci => new CartItemResponseDTO
                {
                    CartItemId = ci.CartItemId,
                    UserId = ci.UserId!,
                    ProductId = ci.ProductId!,
                    ProductName = ci.Product!.Name,
                    ImageUrl = ci.Product.ImageUrl,
                    Quantity = ci.Quantity ?? 0,
                    Price = ci.Product.Price * ci.Quantity,
                    TotalPrice = (ci.Product.Price ?? 0) * (ci.Quantity ?? 0)
                })
                .ToListAsync();

            return cartItems;
        }

        public async Task<string> RemoveFromCartAsync(CartItemDeleteDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.CartItemId))
                return "Thông tin không hợp lệ";

            var item = await _context.CartItems
                .FirstOrDefaultAsync(c => c.CartItemId == dto.CartItemId);

            if (item == null)
                return "Không tìm thấy sản phẩm để xóa";

            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();

            return "Đã xóa sản phẩm khỏi giỏ hàng";
        }

        public async Task<string> UpdateCartItemAsync(CartItemUpdateDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.CartItemId) || dto.Quantity <= 0)
                return "Thông tin không hợp lệ";

            var item = await _context.CartItems
                .FirstOrDefaultAsync(c => c.CartItemId == dto.CartItemId);

            if (item == null)
                return "Không tìm thấy sản phẩm trong giỏ";

            item.Quantity = dto.Quantity;
            await _context.SaveChangesAsync();

            return "Đã cập nhật số lượng sản phẩm";
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Web_Book_BE.DTO;
using Web_Book_BE.Models;

namespace Web_Book_BE.Services.Interfaces
{
    public interface ICartItemService
    {
        Task<List<CartItemResponseDTO>> GetCartByUserAsync(string userId);
        Task<bool> AddToCartAsync(CartItemCreateDTO dto);
        Task<string> UpdateCartItemAsync(CartItemUpdateDTO dto);
        Task<string> RemoveFromCartAsync(CartItemDeleteDTO dto);

    }
}

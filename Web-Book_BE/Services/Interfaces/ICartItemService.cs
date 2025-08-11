using Microsoft.AspNetCore.Mvc;
using Web_Book_BE.DTO;
using Web_Book_BE.Models;

namespace Web_Book_BE.Services.Interfaces
{
    public interface ICartItemService
    {
        Task<List<CartItemResponseDTO>> GetCartByUser(CartItemByUserDTO dto);
       
    }
}

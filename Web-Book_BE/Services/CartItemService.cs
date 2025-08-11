using Microsoft.EntityFrameworkCore;
using System;
using Web_Book_BE.DTO;
using Web_Book_BE.Models;
using Web_Book_BE.Services.Interfaces;

namespace Web_Book_BE.Services
{
    public class CartItemService : ICartItemService
    {
        public Task<List<CartItemResponseDTO>> GetCartByUser(CartItemByUserDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}

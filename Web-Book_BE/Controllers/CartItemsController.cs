using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Web_Book_BE.DTO;
using Web_Book_BE.Models;
using Web_Book_BE.Services.Interfaces;
using Web_Book_BE.Utils;

namespace Web_Book_BE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartItemController : ControllerBase
    {
        private readonly ICartItemService _cartItemService;
       public CartItemController(ICartItemService cartItemService)
        {
            _cartItemService = cartItemService;
        }

        [HttpPost("get-by-user")]
        public async Task<IActionResult> GetCartByUser([FromBody] CartItemByUserDTO dto)
        {
            var cart = await _cartItemService.GetCartByUserAsync(dto.UserId);
            return Ok(cart);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromBody] CartItemCreateDTO dto)
        {
            bool success = await _cartItemService.AddToCartAsync(dto);
            return success ? Ok("Đã thêm sản phẩm vào giỏ hàng") : BadRequest("Thông tin không hợp lệ");
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateCartItem([FromBody] CartItemUpdateDTO dto)
        {
            var message = await _cartItemService.UpdateCartItemAsync(dto);

            return message == "Đã cập nhật số lượng sản phẩm"
                ? Ok(message)
                : BadRequest(message);
        }

        [HttpDelete("remove")]
        public async Task<IActionResult> RemoveFromCart([FromBody] CartItemDeleteDTO dto)
        {
            var message = await _cartItemService.RemoveFromCartAsync(dto);

            return message == "Đã xóa sản phẩm khỏi giỏ hàng"
                ? Ok(message)
                : BadRequest(message);
        }
    }
}

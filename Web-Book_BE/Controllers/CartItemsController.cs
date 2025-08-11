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
        private readonly BookStoreDbContext _context;
        private readonly ICartItemService _cartItemService;
       public CartItemController(ICartItemService cartItemService)
        {
            _cartItemService = cartItemService;
        }
        public CartItemController(BookStoreDbContext context)
        {
            _context = context;
        }
        
        //Lấy giỏ hàng theo UserId
        [HttpPost("by-user")]
        public async Task<IActionResult> GetCartByUser([FromBody] CartItemByUserDTO dto)
        {
            var items = await _cartItemService.GetCartByUser(dto);
            return Ok(items);
        }

        //Thêm sản phẩm vào giỏ
        [HttpPost]
        public IActionResult AddToCart([FromBody] CartItemCreateDTO dto)
        {
            if (string.IsNullOrEmpty(dto.UserId) || string.IsNullOrEmpty(dto.ProductId) || dto.Quantity <= 0)
                return BadRequest("Thông tin không hợp lệ");

            var existingItem = _context.CartItems
                .FirstOrDefault(c => c.UserId == dto.UserId && c.ProductId == dto.ProductId);

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
                _context.CartItems.Add(newItem);
            }

            _context.SaveChanges();
            return Ok("Đã thêm sản phẩm vào giỏ hàng");
        }

        //Cập nhật số lượng sản phẩm
        [HttpPut]
        public IActionResult UpdateCartItem([FromBody] CartItemUpdateDTO dto)
        {
            if (string.IsNullOrEmpty(dto.CartItemId) || dto.Quantity <= 0)
                return BadRequest("Thông tin không hợp lệ");

            var item = _context.CartItems.FirstOrDefault(c => c.CartItemId == dto.CartItemId);
            if (item == null)
                return NotFound("Không tìm thấy sản phẩm trong giỏ");

            item.Quantity = dto.Quantity;
            _context.SaveChanges();

            return Ok("Đã cập nhật số lượng sản phẩm");
        }

        //Xóa sản phẩm khỏi giỏ
        [HttpDelete]
        public IActionResult RemoveFromCart([FromBody] CartItemDeleteDTO dto)
        {
            if (string.IsNullOrEmpty(dto.CartItemId))
                return BadRequest("Thông tin không hợp lệ");

            var item = _context.CartItems.FirstOrDefault(c => c.CartItemId == dto.CartItemId);
            if (item == null)
                return NotFound("Không tìm thấy sản phẩm để xóa");

            _context.CartItems.Remove(item);
            _context.SaveChanges();

            return Ok("Đã xóa sản phẩm khỏi giỏ hàng");
        }
    }
}

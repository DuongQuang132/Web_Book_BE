using Microsoft.AspNetCore.Mvc;
using Web_Book_BE.DTO;
using Web_Book_BE.Models;
using Web_Book_BE.Utils;

namespace Web_Book_BE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly BookStoreDbContext _context;

        public UserController(BookStoreDbContext context)
        {
            _context = context;
        }

        //Đăng ký tài khoản
        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRegisterDTO dto)
        {
            if (_context.Users.Any(u => u.Username == dto.Username))
                return BadRequest("Tên đăng nhập đã tồn tại");

            var user = new User
            {
                UserId = "USR" + IdGenerator.RandomDigits(),
                Username = dto.Username,
                Password = dto.Password, // Có thể mã hóa nếu cần
                Role = dto.Role,
                CustomerId = dto.CustomerId,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok("Đăng ký thành công");
        }

        //Đăng nhập
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginDTO dto)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Username == dto.Username && u.Password == dto.Password && u.IsDeleted != true);

            if (user == null)
                return Unauthorized("Sai tên đăng nhập hoặc mật khẩu");

            var response = new UserResponseDTO
            {
                UserId = user.UserId,
                Username = user.Username,
                Role = user.Role ?? "",
                CustomerId = user.CustomerId ?? "",
                IsDeleted = user.IsDeleted,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };

            return Ok(response);
        }

        //Cập nhật thông tin người dùng
        [HttpPut("update")]
        public IActionResult UpdateUser([FromBody] UserUpdateDTO dto)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == dto.UserId);
            if (user == null)
                return NotFound("Không tìm thấy người dùng");

            user.Username = dto.Username;
            user.Password = dto.Password;
            user.Role = dto.Role;
            user.CustomerId = dto.CustomerId;
            user.UpdatedAt = DateTime.UtcNow;

            _context.SaveChanges();
            return Ok("Cập nhật thành công");
        }

        //Lấy thông tin người dùng theo ID
        [HttpGet("{id}")]
        public IActionResult GetUserById(string id)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == id && u.IsDeleted != true);
            if (user == null)
                return NotFound("Không tìm thấy người dùng");

            var response = new UserResponseDTO
            {
                UserId = user.UserId,
                Username = user.Username,
                Role = user.Role ?? "",
                CustomerId = user.CustomerId ?? "",
                IsDeleted = user.IsDeleted,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };

            return Ok(response);
        }

        //Lấy tất cả người dùng
        [HttpGet("all")]
        public IActionResult GetAllUsers()
        {
            var users = _context.Users
                .Where(u => u.IsDeleted != true)
                .Select(user => new UserResponseDTO
                {
                    UserId = user.UserId,
                    Username = user.Username,
                    Role = user.Role ?? "",
                    CustomerId = user.CustomerId ?? "",
                    IsDeleted = user.IsDeleted,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = user.UpdatedAt
                })
                .ToList();

            return Ok(users);
        }

        //Xóa mềm người dùng
        [HttpPut("delete")]
        public IActionResult SoftDeleteUser([FromBody] UserDeleteDTO dto)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == dto.UserId);
            if (user == null)
                return NotFound("Không tìm thấy người dùng");

            user.IsDeleted = true;
            user.UpdatedAt = DateTime.UtcNow;

            _context.SaveChanges();
            return Ok("Người dùng đã được xóa mềm");
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Web_Book_BE.DTO;
using Web_Book_BE.Models;
using Web_Book_BE.Services.Interfaces;
using Web_Book_BE.Utils;
using static System.Net.Mime.MediaTypeNames;
using BCrypt.Net;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using CloudinaryDotNet.Core;

namespace Web_Book_BE.Services
{
    public class UserService : IUserService
    {
        private readonly BookStoreDbContext _context;
   
     private readonly IConfiguration _configuration;
        //Đăng Ký
        public UserService(BookStoreDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<string> CreateUserAsync(UserRegisterDTO dto)
        {
            var exists = await _context.Users
                .AnyAsync(u => u.Username == dto.Username);

            if (exists)
                return "Tên đăng nhập đã tồn tại";

            var user = new User
            {
                UserId = "USR" + IdGenerator.RandomDigits(),
                Username = dto.Username,
                Password = dto.Password,
                Role = dto.Role,
                CustomerId = dto.CustomerId,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Users.AddAsync(user);
            var result = await _context.SaveChangesAsync();

            return result > 0
                ? "Đăng ký thành công"
                : "Đăng ký thất bại";
        }
        public async Task<ApiResponse<UserLoginResponseDTO>> LoginAsync(UserLoginDTO dto)
        {
            // 🔎 Tìm user theo Username
            var user = await _context.Users
                .Include(u => u.Customer)
                .FirstOrDefaultAsync(u =>
                    u.Username == dto.Username &&
                    u.IsDeleted == false);

            if (user == null)
            {
                return new ApiResponse<UserLoginResponseDTO>
                {
                    Status = 400,
                    Message = "Sai tài khoản hoặc mật khẩu",
                    Data = null
                };
            }

            // ⚠️ Nếu bạn đang lưu password dạng hash thì thay bằng verify hash
            if (user.Password != dto.Password)
            {
                return new ApiResponse<UserLoginResponseDTO>
                {
                    Status = 400,
                    Message = "Sai tài khoản hoặc mật khẩu",
                    Data = null
                };
            }

            // 🔑 Tạo JWT Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role ?? "")
        }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // 📦 Trả về DTO có token trong ApiResponse
            return new ApiResponse<UserLoginResponseDTO>
            {
                Status = 200,
                Message = "Login thành công",
                Data = new UserLoginResponseDTO
                {
                    UserId = user.UserId,
                    Username = user.Username,
                    Role = user.Role ?? "",
                    Address = user.Customer.Address,
                    Token = tokenString
                }
            };
        }

        public async Task<string> UpdateUserAsync(UserUpdateDTO dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == dto.UserId);

            if (user == null)
                return "Không tìm thấy người dùng";

            var usernameExists = await _context.Users
                .AnyAsync(u => u.Username == dto.Username && u.UserId != dto.UserId);

            if (usernameExists)
                return "Tên đăng nhập đã tồn tại";

            user.Username = dto.Username;
            user.Role = dto.Role;
            user.CustomerId = dto.CustomerId;
            user.UpdatedAt = DateTime.UtcNow;

            if (!string.IsNullOrWhiteSpace(dto.Password))
                user.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var result = await _context.SaveChangesAsync();

            return result > 0
                ? "Cập nhật thành công"
                : "Cập nhật thất bại";
        }
        public async Task<UserResponseDTO?> GetUserByIdAsync(string id)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == id && u.IsDeleted != true);

            if (user == null)
                return null;

            return new UserResponseDTO
            {
                UserId = user.UserId,
                Username = user.Username,
                Role = user.Role ?? "",
                CustomerId = user.CustomerId ?? "",
                IsDeleted = user.IsDeleted,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };
        }
        public async Task<List<UserResponseDTO>> GetAllUsersAsync()
        {
            return await _context.Users
                .Where(u => u.IsDeleted != true)
                .Select(user => new UserResponseDTO
                {
                    UserId = user.UserId,
                    Username = user.Username,
                    Password = user.Password,
                    Role = user.Role ?? "",
                    CustomerId = user.CustomerId ?? "",
                    IsDeleted = user.IsDeleted,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = user.UpdatedAt
                })
                .ToListAsync();
        }
        public async Task<string> IDeleteUserAsync(UserDeleteDTO dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == dto.UserId);

            if (user == null)
                return "Không tìm thấy người dùng";

            user.IsDeleted = true;
            user.UpdatedAt = DateTime.UtcNow;

            var result = await _context.SaveChangesAsync();

            return result > 0
                ? "Người dùng đã được xóa mềm"
                : "Xóa mềm thất bại";
        }

    }
}

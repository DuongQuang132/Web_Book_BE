using Microsoft.EntityFrameworkCore;
using Web_Book_BE.DTO;
using Web_Book_BE.Models;
using Web_Book_BE.Services.Interfaces;
using Web_Book_BE.Utils;
using static System.Net.Mime.MediaTypeNames;
using BCrypt.Net;

namespace Web_Book_BE.Services
{
    public class UserService : IUserService
    {
        private readonly BookStoreDbContext _context;

        public UserService(BookStoreDbContext context)
        {
            _context = context;
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
        public async Task<UserResponseDTO?> LoginAsync(UserLoginDTO dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u =>
                u.Username == dto.Username &&
                u.Password == dto.Password &&
                u.IsDeleted == false);

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

namespace Web_Book_BE.DTO
{
    // Dùng khi đăng ký tài khoản người dùng
    public class UserRegisterDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string CustomerId { get; set; }
    }

    // Dùng khi đăng nhập
    public class UserLoginDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    // Dùng khi cập nhật thông tin người dùng
    public class UserUpdateDTO
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string CustomerId { get; set; }
    }

    // Dùng để trả về dữ liệu người dùng cho client
    public class UserResponseDTO
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public string CustomerId { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    // Dùng để xóa mềm người dùng
    public class UserDeleteDTO
    {
        public string UserId { get; set; }
    }

    // Dùng để truy vấn theo ID
    public class UserByIdDTO
    {
        public string UserId { get; set; }
    }
}

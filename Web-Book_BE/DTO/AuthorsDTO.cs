namespace Web_Book_BE.DTO
{
    // Dùng khi tạo mới tác giả
    public class AuthorCreateDTO
    {
        public string AuthorName { get; set; }
        public string Bio { get; set; }
    }

    // Dùng khi cập nhật thông tin tác giả
    public class AuthorUpdateDTO
    {
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string Bio { get; set; }
    }

    // Dùng để trả về dữ liệu tác giả cho client
    public class AuthorResponseDTO
    {
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string Bio { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    // Dùng để truy vấn theo ID
    public class AuthorByIdDTO
    {
        public string AuthorId { get; set; }
    }
}

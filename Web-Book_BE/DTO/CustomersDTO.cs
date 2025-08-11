namespace Web_Book_BE.DTO
{
    // Dùng khi tạo mới thông tin khách hàng
    public class CustomerCreateDTO
    {
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Address { get; set; }
    }

    // Dùng khi cập nhật thông tin khách hàng
    public class CustomerUpdateDTO
    {
        public string CustomerId { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Address { get; set; }
    }

    // Dùng để trả về dữ liệu khách hàng cho client
    public class CustomerResponseDTO
    {
        public string CustomerId { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Address { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    // Dùng để truy vấn theo ID
    public class CustomerByIdDTO
    {
        public string CustomerId { get; set; }
    }
}

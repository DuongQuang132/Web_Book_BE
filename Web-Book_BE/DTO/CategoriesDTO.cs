namespace Web_Book_BE.DTO
{
    // Dùng khi tạo mới danh mục
    public class CategoryCreateDTO
    {
        public string CategoriesName { get; set; }
        public string Description { get; set; }
    }

    // Dùng khi cập nhật danh mục
    public class CategoryUpdateDTO
    {
        public string CategoriesId { get; set; }
        public string CategoriesName { get; set; }
        public string Description { get; set; }
    }

    // Dùng để trả về dữ liệu danh mục cho client
    public class CategoryResponseDTO
    {
        public string CategoriesId { get; set; }
        public string CategoriesName { get; set; }
        public string Description { get; set; }
    }

    // Dùng để truy vấn danh mục theo ID
    public class CategoryByIdDTO
    {
        public string CategoriesId { get; set; }
    }
}

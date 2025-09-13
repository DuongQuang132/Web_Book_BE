using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Book_BE.DTO;
using Web_Book_BE.Models;
using Web_Book_BE.Services.Interfaces;
using Web_Book_BE.Utils;

namespace Web_Book_BE.Services
{
    public class AuthorsService : IAuthorService
    {
        private readonly BookStoreDbContext _context;

        public AuthorsService(BookStoreDbContext context)
        {
            _context = context;
        }
        public async Task<string> CreateAuthorAsync(AuthorCreateDTO dto)
        {
            var author = new Authors
            {
                AuthorId = "TG" + IdGenerator.RandomDigits(),
                AuthorName = dto.AuthorName,
                Bio = dto.Bio,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            return "Author created successfully";
        }

        public async Task<List<AuthorResponseDTO>> GetAllAuthorsAsync()
        {
            return await _context.Authors
                .Select(a => new AuthorResponseDTO
                {
                    AuthorId = a.AuthorId,
                    AuthorName = a.AuthorName,
                    Bio = a.Bio,
                    CreatedAt = a.CreatedAt,
                    UpdatedAt = a.UpdatedAt
                })
                .ToListAsync();
        }
        public async Task<IActionResult> GetAuthorByIdAsync(string id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null)
                return new NotFoundResult();

            var dto = new AuthorResponseDTO
            {
                AuthorId = author.AuthorId,
                AuthorName = author.AuthorName,
                Bio = author.Bio,
                CreatedAt = author.CreatedAt,
                UpdatedAt = author.UpdatedAt
            };

            return new OkObjectResult(dto);
        }


        public async Task<string> RemoveAuthor(string id)
        {
            var author = await _context.Authors
                .FirstOrDefaultAsync(c => c.AuthorId == id);

            if (author == null)
                return "Không tìm thấy tác giả để xóa"; // Fixed: Changed from "danh mục" to "tác giả"

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            return "Tác giả đã được xóa";
        }

        public async Task<IActionResult> UpdateAuthorAsync(AuthorUpdateDTO dto)
        {
            var author = await _context.Authors
                .FirstOrDefaultAsync(a => a.AuthorId == dto.AuthorId);

            if (author == null)
            {
                return new NotFoundObjectResult("Không tìm thấy tác giả");
            }

            author.AuthorName = dto.AuthorName;
            author.Bio = dto.Bio;
            author.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new OkObjectResult("Cập nhật tác giả thành công");
        }
    }
}

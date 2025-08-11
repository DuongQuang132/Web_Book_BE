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
        public async  Task<string> CreateAuthor(AuthorCreateDTO dto)
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

        public Task<List<AuthorResponseDTO>> GetAllAuthors()
        {
            throw new NotImplementedException();
        }

        public Task<AuthorResponseDTO> GetAuthorById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<string> UpdateAuthor(AuthorUpdateDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}

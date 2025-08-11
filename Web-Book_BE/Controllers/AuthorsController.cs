using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Web_Book_BE.DTO;
using Web_Book_BE.Models;
using Web_Book_BE.Services.Interfaces;
using Web_Book_BE.Utils;

namespace Web_Book_BE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorsController : ControllerBase
    {
 
        //LẤY SERVICE
        private readonly IAuthorService _authorItemService;
        public AuthorsController( IAuthorService authorItemService)
        {
  
            _authorItemService = authorItemService;
        }

        // POST: api/Authors
        [HttpPost]
        public async Task<IActionResult> CreateAuthor([FromBody] AuthorCreateDTO dto)
        {
            var items = await _authorItemService.CreateAuthor(dto);
            return Ok(items);
        }
      

        // PUT: api/Authors
        [HttpPut]
        public IActionResult UpdateAuthor([FromBody] AuthorUpdateDTO dto)
        {
            var author = _context.Authors.FirstOrDefault(a => a.AuthorId == dto.AuthorId);
            if (author == null)
            {
                return NotFound("Author not found");
            }

            author.AuthorName = dto.AuthorName;
            author.Bio = dto.Bio;
            author.UpdatedAt = DateTime.UtcNow;

            _context.SaveChanges();

            return Ok("Author updated successfully");
        }

        // GET: api/Authors/{id}
        [HttpGet("{id}")]
        public IActionResult GetAuthorById(string id)
        {
            var author = _context.Authors.FirstOrDefault(a => a.AuthorId == id);
            if (author == null)
            {
                return NotFound("Author not found");
            }

                var response = new AuthorResponseDTO
                {
                    AuthorId = author.AuthorId,
                    AuthorName = author.AuthorName,
                    Bio = author.Bio,
                    CreatedAt = author.CreatedAt,
                    UpdatedAt = author.UpdatedAt
                };

            return Ok(response);
        }

        // GET: api/Authors
        [HttpGet]
        public IActionResult GetAllAuthors()
        {
            var authors = _context.Authors
                .Select(a => new AuthorResponseDTO
                {
                    AuthorId = a.AuthorId,
                    AuthorName = a.AuthorName,
                    Bio = a.Bio,
                    CreatedAt = a.CreatedAt,
                    UpdatedAt = a.UpdatedAt
                })
                .ToList();

            return Ok(authors);
        }
    }
}

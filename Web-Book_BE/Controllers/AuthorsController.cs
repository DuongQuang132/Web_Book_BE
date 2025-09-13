using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Web_Book_BE.DTO;
using Web_Book_BE.Models;
using Web_Book_BE.Services;
using Web_Book_BE.Services.Interfaces;
using Web_Book_BE.Utils;

namespace Web_Book_BE.Controllers
{
    [ApiController]
    [Route("author")]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorService _authorItemService;
        public AuthorsController( IAuthorService authorItemService)
        { 
            _authorItemService = authorItemService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAuthor([FromBody] AuthorCreateDTO dto)
        {
            var items = await _authorItemService.CreateAuthorAsync(dto);
            return Ok(items);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAuthor([FromBody] AuthorUpdateDTO dto)
        {
            return await _authorItemService.UpdateAuthorAsync(dto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuthorById(string id)
        {
            return await _authorItemService.GetAuthorByIdAsync(id);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAuthors()
        {
            var authors = await _authorItemService.GetAllAuthorsAsync();
            return Ok(authors);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(string id)
        {
            var message = await _authorItemService.RemoveAuthor(id);

         
            return message == "Tác giả đã được xóa" || message == "Danh mục đã được xóa"
                ? Ok(message)
                : NotFound(message);
        }
    }
}

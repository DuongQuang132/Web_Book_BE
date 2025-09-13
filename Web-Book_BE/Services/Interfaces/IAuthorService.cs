using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Web_Book_BE.DTO;

namespace Web_Book_BE.Services.Interfaces
{
    public interface IAuthorService
    {
        Task<string> CreateAuthorAsync(AuthorCreateDTO dto);
        Task<IActionResult> UpdateAuthorAsync(AuthorUpdateDTO dto);
        Task<IActionResult> GetAuthorByIdAsync(string id);
        Task<List<AuthorResponseDTO>> GetAllAuthorsAsync();
        Task<string> RemoveAuthor(string id);
    }
}

using Microsoft.AspNetCore.Mvc;
using Web_Book_BE.DTO;

namespace Web_Book_BE.Services.Interfaces
{
    public interface IAuthorService
    {
        //Task là giá trị trả về + object + giá trị truyền vào
        Task<string> CreateAuthor(AuthorCreateDTO dto);
        Task<string> UpdateAuthor(AuthorUpdateDTO dto);
        Task<AuthorResponseDTO> GetAuthorById(string id);
        Task<List<AuthorResponseDTO>> GetAllAuthors();
    }
}

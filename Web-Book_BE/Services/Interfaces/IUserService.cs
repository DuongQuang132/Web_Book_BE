using Web_Book_BE.DTO;

namespace Web_Book_BE.Services.Interfaces
{
    public interface IUserService
    {
        Task<string> CreateUserAsync(UserRegisterDTO dto);
        Task<UserResponseDTO?> LoginAsync(UserLoginDTO dto);
        Task<string> UpdateUserAsync(UserUpdateDTO dto);
        Task<UserResponseDTO?> GetUserByIdAsync(string id);
        Task<List<UserResponseDTO>> GetAllUsersAsync();
        Task<string> IDeleteUserAsync(UserDeleteDTO dto);
    }
}


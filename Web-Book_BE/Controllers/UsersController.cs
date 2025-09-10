using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks.Dataflow;
using Web_Book_BE.DTO;
using Web_Book_BE.Models;
using Web_Book_BE.Services.Interfaces;
using Web_Book_BE.Utils;

namespace Web_Book_BE.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController (IUserService userService)
        {
            _userService = userService;
        }
        //Đăng ký tài khoản
        [HttpPost("register")]
        public async Task<IActionResult> CreateUser([FromBody] UserRegisterDTO dto)
        {
            var message = await _userService.CreateUserAsync(dto);

            return Ok(message);
        }

        //Đăng nhập
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO dto)
        {
            var response = await _userService.LoginAsync(dto);

            return response != null
                ? Ok(response)
                : Unauthorized("Sai tên đăng nhập hoặc mật khẩu");
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateDTO dto)
        {
            var message = await _userService.UpdateUserAsync(dto);

            return message switch
            {
                "Cập nhật thành công" => Ok(message),
                "Tên đăng nhập đã tồn tại" => BadRequest(message),
                "Không tìm thấy người dùng" => NotFound(message),
                _ => StatusCode(500, message)
            };
        }

        //Lấy thông tin người dùng theo ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var userDto = await _userService.GetUserByIdAsync(id);

            if (userDto == null)
                return NotFound("Không tìm thấy người dùng");

            return Ok(userDto);
        }

        //Lấy tất cả người dùng
        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }


        //Xóa mềm người dùng
        [HttpPut("delete")]
        public async Task<IActionResult> IDeleteUser([FromBody] UserDeleteDTO dto)
        {
            var message = await _userService.IDeleteUserAsync(dto);

            return message switch
            {
                "Người dùng đã được xóa mềm" => Ok(message),
                "Không tìm thấy người dùng" => NotFound(message),
                _=> StatusCode(500, message)
            };
        }

    }
}

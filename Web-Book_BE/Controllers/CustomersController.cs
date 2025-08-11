using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Book_BE.DTO;
using Web_Book_BE.Models;
using Web_Book_BE.Utils;

namespace Web_Book_BE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly BookStoreDbContext _context;

        public CustomersController(BookStoreDbContext context)
        {
            _context = context;
        }

        //Tạo mới khách hàng
        [HttpPost]
        public IActionResult CreateCustomer([FromBody] CustomerCreateDTO dto)
        {
            if (string.IsNullOrEmpty(dto.FullName) || string.IsNullOrEmpty(dto.Phone))
                return BadRequest("Vui lòng nhập đầy đủ họ tên và số điện thoại");

            var customer = new Customers
            {
                CustomerId = "CUS" + IdGenerator.RandomDigits(),
                FullName = dto.FullName,
                Phone = dto.Phone,
                Email = dto.Email,
                Gender = dto.Gender,
                BirthDate = dto.BirthDate.HasValue
                    ? DateOnly.FromDateTime(dto.BirthDate.Value)
                    : null,
                Address = dto.Address,
                CreatedAt = DateTime.UtcNow
            };

            _context.Customers.Add(customer);
            _context.SaveChanges();

            return Ok("Khách hàng đã được tạo thành công");
        }

        //Cập nhật thông tin khách hàng
        [HttpPut]
        public IActionResult UpdateCustomer([FromBody] CustomerUpdateDTO dto)
        {
            var customer = _context.Customers.FirstOrDefault(c => c.CustomerId == dto.CustomerId);
            if (customer == null)
                return NotFound("Không tìm thấy khách hàng");

            customer.FullName = dto.FullName;
            customer.Phone = dto.Phone;
            customer.Email = dto.Email;
            customer.Gender = dto.Gender;
            customer.BirthDate = dto.BirthDate.HasValue
                ? DateOnly.FromDateTime(dto.BirthDate.Value)
                : null;
            customer.Address = dto.Address;
            customer.UpdatedAt = DateTime.UtcNow;

            _context.SaveChanges();
            return Ok("Thông tin khách hàng đã được cập nhật");
        }

        //Xóa khách hàng
        [HttpDelete("{id}")]
        public IActionResult DeleteCustomer(string id)
        {
            var customer = _context.Customers.FirstOrDefault(c => c.CustomerId == id);
            if (customer == null)
                return NotFound("Không tìm thấy khách hàng để xóa");

            _context.Customers.Remove(customer);
            _context.SaveChanges();

            return Ok("Khách hàng đã được xóa");
        }

        //Lấy thông tin khách hàng theo ID
        [HttpGet("{id}")]
        public IActionResult GetCustomerById(string id)
        {
            var customer = _context.Customers.FirstOrDefault(c => c.CustomerId == id);
            if (customer == null)
                return NotFound("Không tìm thấy khách hàng");

            var response = new CustomerResponseDTO
            {
                CustomerId = customer.CustomerId,
                FullName = customer.FullName,
                Phone = customer.Phone,
                Email = customer.Email,
                Gender = customer.Gender,
                BirthDate = customer.BirthDate.HasValue
                    ? customer.BirthDate.Value.ToDateTime(TimeOnly.MinValue)
                    : null,
                Address = customer.Address,
                CreatedAt = customer.CreatedAt,
                UpdatedAt = customer.UpdatedAt
            };

            return Ok(response);
        }

        //Lấy tất cả khách hàng
        [HttpGet]
        public IActionResult GetAllCustomers()
        {
            var customers = _context.Customers
                .Select(c => new CustomerResponseDTO
                {
                    CustomerId = c.CustomerId,
                    FullName = c.FullName,
                    Phone = c.Phone,
                    Email = c.Email,
                    Gender = c.Gender,
                    BirthDate = c.BirthDate.HasValue
                        ? c.BirthDate.Value.ToDateTime(TimeOnly.MinValue)
                        : null,
                    Address = c.Address,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt
                })
                .ToList();

            return Ok(customers);
        }
    }
}

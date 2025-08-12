using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Web_Book_BE.DTO;
using Web_Book_BE.Models;
using Web_Book_BE.Utils;
using Web_Book_BE.Services.Interfaces;

namespace Web_Book_BE.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly BookStoreDbContext _context;

        public CustomerService(BookStoreDbContext context)
        {
            _context = context;
        }

        public async Task<string> CreateCustomerAsync(CustomerCreateDTO dto)
        {
            if (string.IsNullOrEmpty(dto.FullName) || string.IsNullOrEmpty(dto.Phone))
                throw new ArgumentException("Vui lòng nhập đầy đủ họ tên và số điện thoại");

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
            await _context.SaveChangesAsync();

            return "Khách hàng đã được tạo thành công";
        }

        public async Task<string> UpdateCustomerAsync(CustomerUpdateDTO dto)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.CustomerId == dto.CustomerId);
            if (customer == null)
                throw new KeyNotFoundException("Không tìm thấy khách hàng");

            customer.FullName = dto.FullName;
            customer.Phone = dto.Phone;
            customer.Email = dto.Email;
            customer.Gender = dto.Gender;
            customer.BirthDate = dto.BirthDate.HasValue
                ? DateOnly.FromDateTime(dto.BirthDate.Value)
                : null;
            customer.Address = dto.Address;
            customer.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return "Thông tin khách hàng đã được cập nhật";
        }

        public async Task<string> DeleteCustomerAsync(string id)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.CustomerId == id);
            if (customer == null)
                throw new KeyNotFoundException("Không tìm thấy khách hàng để xóa");

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return "Khách hàng đã được xóa";
        }

        public async Task<CustomerResponseDTO> GetCustomerByIdAsync(string id)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.CustomerId == id);
            if (customer == null)
                throw new KeyNotFoundException("Không tìm thấy khách hàng");

            return new CustomerResponseDTO
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
        }

        public async Task<List<CustomerResponseDTO>> GetAllCustomersAsync()
        {
            return await _context.Customers
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
                .ToListAsync();
        }
    }
}
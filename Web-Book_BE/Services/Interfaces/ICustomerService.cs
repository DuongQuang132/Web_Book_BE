using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web_Book_BE.DTO;
using Web_Book_BE.Models;
namespace Web_Book_BE.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<string> CreateCustomerAsync(CustomerCreateDTO dto);
        Task<string> UpdateCustomerAsync(CustomerUpdateDTO dto);
        Task<string> DeleteCustomerAsync(string id);
        Task<CustomerResponseDTO> GetCustomerByIdAsync(string id);
        Task<List<CustomerResponseDTO>> GetAllCustomersAsync();
    }
}

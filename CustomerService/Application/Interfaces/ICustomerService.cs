using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerService.Domain.DTOs.Customer;

namespace CustomerService.Application.Interfaces
{
    public interface ICustomerService
    {
        Task<List<CustomerResponseDto>> GetAllCustomersAsync(CustomerQueryDto query);
        Task<CustomerResponseDto> GetCustomerByIdAsync(Guid id);
        Task<CustomerResponseDto> CreateCustomerAsync(CustomerCreateDto createDto);
        Task<CustomerResponseDto> UpdateCustomerAsync(Guid id, CustomerUpdateDto updateDto);
        Task<bool> DeleteCustomerAsync(Guid id);
    }
}
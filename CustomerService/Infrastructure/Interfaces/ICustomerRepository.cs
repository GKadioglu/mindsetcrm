using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerService.Domain.Entities;

namespace CustomerService.Infrastructure.Interfaces
{
    public interface ICustomerRepository
    {
        Task<List<Customer>> GetAllAsync();
        Task<Customer> GetByIdAsync(Guid id);
        Task CreateAsync(Customer customer);
        Task UpdateAsync(Customer customer);
        Task<bool> DeleteAsync(Guid id);
    }
}
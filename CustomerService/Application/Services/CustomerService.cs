using CustomerService.Application.Constants;
using CustomerService.Application.Interfaces;
using CustomerService.Domain.DTOs.Customer;
using CustomerService.Domain.Entities;
using CustomerService.Infrastructure.Interfaces;

namespace CustomerService.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<List<CustomerResponseDto>> GetAllCustomersAsync(CustomerQueryDto query)
        {
            var customers = await _customerRepository.GetAllAsync();

            // Filtreleme
            if (!string.IsNullOrWhiteSpace(query.Company))
                customers = customers.Where(c => c.Company.Contains(query.Company, StringComparison.OrdinalIgnoreCase)).ToList();

            if (!string.IsNullOrWhiteSpace(query.FullName))
                customers = customers.Where(c => c.FullName.Contains(query.FullName, StringComparison.OrdinalIgnoreCase)).ToList();

            // Sıralama
            customers = query.SortBy?.ToLower() switch
            {
                "fullname" => query.SortOrder == "desc"
                    ? customers.OrderByDescending(c => c.FullName).ToList()
                    : customers.OrderBy(c => c.FullName).ToList(),

                "email" => query.SortOrder == "desc"
                    ? customers.OrderByDescending(c => c.Email).ToList()
                    : customers.OrderBy(c => c.Email).ToList(),

                "company" => query.SortOrder == "desc"
                    ? customers.OrderByDescending(c => c.Company).ToList()
                    : customers.OrderBy(c => c.Company).ToList(),

                _ => customers
            };

            return customers.Select(c => new CustomerResponseDto
            {
                Id = c.Id,
                FullName = c.FullName,
                Email = c.Email,
                Phone = c.Phone,
                Company = c.Company
            }).ToList();
        }

        public async Task<CustomerResponseDto> GetCustomerByIdAsync(Guid id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null)
                throw new Exception(ErrorMessages.CustomerNotFound);

            return new CustomerResponseDto
            {
                Id = customer.Id,
                FullName = customer.FullName,
                Email = customer.Email,
                Phone = customer.Phone,
                Company = customer.Company
            };
        }

        public async Task<CustomerResponseDto> CreateCustomerAsync(CustomerCreateDto createDto)
        {
            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                FullName = createDto.FullName,
                Email = createDto.Email,
                Phone = createDto.Phone,
                Company = createDto.Company
            };

            await _customerRepository.CreateAsync(customer);


            return new CustomerResponseDto
            {
                Id = customer.Id,
                FullName = customer.FullName,
                Email = customer.Email,
                Phone = customer.Phone,
                Company = customer.Company
            };
        }

        public async Task<CustomerResponseDto> UpdateCustomerAsync(Guid id, CustomerUpdateDto updateDto)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null)
                throw new Exception(ErrorMessages.CustomerNotFound);

            customer.FullName = updateDto.FullName;
            customer.Email = updateDto.Email;
            customer.Phone = updateDto.Phone;
            customer.Company = updateDto.Company;

            await _customerRepository.UpdateAsync(customer);

            return new CustomerResponseDto
            {
                Id = customer.Id,
                FullName = customer.FullName,
                Email = customer.Email,
                Phone = customer.Phone,
                Company = customer.Company
            };
        }

        public async Task<bool> DeleteCustomerAsync(Guid id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null)
                throw new Exception(ErrorMessages.CustomerNotFound);

            return await _customerRepository.DeleteAsync(id);
        }
    }
}

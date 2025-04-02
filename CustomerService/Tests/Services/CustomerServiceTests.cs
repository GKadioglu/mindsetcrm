using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CustomerService.Application.Interfaces;
using CustomerService.Application.Services;
using CustomerService.Domain.DTOs;
using CustomerService.Domain.Entities;
using CustomerService.Infrastructure.Interfaces;
using CustomerService.Domain.DTOs.Customer;

namespace CustomerService.Tests.Services
{
    public class CustomerServiceTests
    {
        private readonly Mock<ICustomerRepository> _mockRepo;
        private readonly ICustomerService _customerService;

        public CustomerServiceTests()
        {
            _mockRepo = new Mock<ICustomerRepository>();
            _customerService = new CustomerService.Application.Services.CustomerService(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAllCustomersAsync_ShouldReturnCustomers()
        {
            var customers = new List<Customer>
        {
            new Customer { Id = Guid.NewGuid(), FullName = "Ali Veli", Email = "ali@veli.com", Company = "Test AŞ", Phone = "05550001122" },
            new Customer { Id = Guid.NewGuid(), FullName = "Ayşe Fatma", Email = "ayse@fatma.com", Company = "Demo Ltd", Phone = "05550003344" }
        };

            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(customers);

            var query = new CustomerQueryDto();

            var result = await _customerService.GetAllCustomersAsync(query);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetCustomerByIdAsync_ValidId_ReturnsCustomer()
        {

            var customerId = Guid.NewGuid();

            var customer = new Customer
            {
                Id = customerId,
                FullName = "Test Müşteri",
                Email = "test@domain.com",
                Phone = "05550000000",
                Company = "Test Şirketi",
                Notes = new List<CustomerNote>()
            };

            _mockRepo.Setup(r => r.GetByIdAsync(customerId)).ReturnsAsync(customer);

            var result = await _customerService.GetCustomerByIdAsync(customerId);

            Assert.NotNull(result);
            Assert.Equal(customerId, result.Id);
            Assert.Equal(customer.FullName, result.FullName);
            Assert.Equal(customer.Email, result.Email);
            Assert.Equal(customer.Phone, result.Phone);
            Assert.Equal(customer.Company, result.Company);
        }

        [Fact]
        public async Task CreateCustomerAsync_ValidData_ReturnsCreatedCustomer()
        {
            var createDto = new CustomerCreateDto
            {
                FullName = "Yeni Müşteri",
                Email = "yeni@domain.com",
                Phone = "05551112233",
                Company = "Yeni Şirket"
            };

            var createdCustomer = new Customer
            {
                Id = Guid.NewGuid(),
                FullName = createDto.FullName,
                Email = createDto.Email,
                Phone = createDto.Phone,
                Company = createDto.Company,
                Notes = new List<CustomerNote>(),
            };

            _mockRepo.Setup(r => r.CreateAsync(It.IsAny<Customer>())).Returns(Task.CompletedTask);

            // Act
            var result = await _customerService.CreateCustomerAsync(createDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(createDto.FullName, result.FullName);
            Assert.Equal(createDto.Email, result.Email);
            Assert.Equal(createDto.Phone, result.Phone);
            Assert.Equal(createDto.Company, result.Company);
        }

        [Fact]
        public async Task UpdateCustomerAsync_ValidData_ReturnsUpdatedCustomer()
        {
            var customerId = Guid.NewGuid();

            var existingCustomer = new Customer
            {
                Id = customerId,
                FullName = "Eski İsim",
                Email = "eski@domain.com",
                Phone = "05550000000",
                Company = "Eski Şirket",
                Notes = new List<CustomerNote>()
            };

            var updateDto = new CustomerUpdateDto
            {
                FullName = "Güncel İsim",
                Email = "guncel@domain.com",
                Phone = "05551112233",
                Company = "Yeni Şirket"
            };

            _mockRepo.Setup(r => r.GetByIdAsync(customerId)).ReturnsAsync(existingCustomer);
            _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Customer>())).Returns(Task.CompletedTask);

            var result = await _customerService.UpdateCustomerAsync(customerId, updateDto);

            Assert.NotNull(result);
            Assert.Equal(updateDto.FullName, result.FullName);
            Assert.Equal(updateDto.Email, result.Email);
            Assert.Equal(updateDto.Phone, result.Phone);
            Assert.Equal(updateDto.Company, result.Company);
        }

        [Fact]
        public async Task DeleteCustomerAsync_ValidId_ReturnsTrue()
        {
            var testCustomerId = Guid.NewGuid();
            var testCustomer = new Customer
            {
                Id = testCustomerId,
                FullName = "Test Müşteri",
                Email = "test@example.com",
                Phone = "05551112233",
                Company = "Test Şirket"
            };

            _mockRepo.Setup(r => r.GetByIdAsync(testCustomerId))
                     .ReturnsAsync(testCustomer);

            _mockRepo.Setup(r => r.DeleteAsync(testCustomerId))
                     .ReturnsAsync(true);

            var result = await _customerService.DeleteCustomerAsync(testCustomerId);

            Assert.True(result);
        }

    }
}

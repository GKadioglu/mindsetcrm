using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SalesService.Application.Interfaces;
using SalesService.Application.Services;
using SalesService.Domain.DTOs;
using SalesService.Domain.Entities;
using SalesService.Infrastructure.Interfaces;

namespace SalesService.Tests.Services
{
    public class SalesServiceTests
    {
        private readonly Mock<ISaleRepository> _mockRepo;
        private readonly ISaleService _saleService;

        public SalesServiceTests()
        {
            _mockRepo = new Mock<ISaleRepository>();
            _saleService = new SaleService(_mockRepo.Object);
        }

        [Fact]
        public async Task CreateSaleAsync_ValidData_ReturnsCreatedSale()
        {
            var createDto = new SaleCreateDto
            {
                CustomerName = "Ahmet Yılmaz",
                Product = "Lenovo ThinkPad",
                Amount = 15500
            };

            var createdSale = new Sale
            {
                Id = Guid.NewGuid().ToString(),
                CustomerName = createDto.CustomerName,
                Product = createDto.Product,
                Amount = createDto.Amount,
                Status = "Yeni",
                Notes = new List<SaleNote>(),
                CreatedAt = DateTime.UtcNow,
                LastStatusChange = DateTime.UtcNow
            };

            _mockRepo.Setup(r => r.CreateAsync(It.IsAny<Sale>()))
                     .Returns(Task.CompletedTask);

            _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<string>()))
                     .ReturnsAsync(createdSale);

            var result = await _saleService.CreateSaleAsync(createDto);

            Assert.NotNull(result);
            Assert.Equal(createDto.CustomerName, result.CustomerName);
            Assert.Equal(createDto.Product, result.Product);
            Assert.Equal(createDto.Amount, result.Amount);
        }

        [Fact]
        public async Task UpdateStatusAsync_ValidId_UpdatesStatusAndAddsNote()
        {
            var saleId = Guid.NewGuid().ToString();
            var updateDto = new SaleUpdateStatusDto
            {
                NewStatus = "İletişimde",
                Note = "Müşteriyle telefonla görüşüldü."
            };

            var existingSale = new Sale
            {
                Id = saleId,
                CustomerName = "Mehmet Demir",
                Product = "iPhone 13",
                Amount = 30000,
                Status = "Yeni",
                Notes = new List<SaleNote>(),
                CreatedAt = DateTime.UtcNow,
                LastStatusChange = DateTime.UtcNow
            };

            _mockRepo.Setup(r => r.GetByIdAsync(saleId)).ReturnsAsync(existingSale);
            _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Sale>())).ReturnsAsync(true);

            var result = await _saleService.UpdateStatusAsync(saleId, updateDto);

            Assert.True(result);
            Assert.Equal(updateDto.NewStatus, existingSale.Status);
            Assert.Single(existingSale.Notes);
            Assert.Equal(updateDto.Note, existingSale.Notes[0].Note);
        }
        [Fact]
        public async Task GetAllSalesAsync_ReturnsListOfSales()
        {
            var sales = new List<Sale>
            {
                new Sale
                {
                    Id = Guid.NewGuid().ToString(),
                    CustomerName = "Ahmet Yılmaz",
                    Product = "Laptop",
                    Amount = 15000,
                    Status = "Yeni",
                    CreatedAt = DateTime.UtcNow,
                    LastStatusChange = DateTime.UtcNow,
                    Notes = new List<SaleNote>()
                },
                new Sale
                {
                    Id = Guid.NewGuid().ToString(),
                    CustomerName = "Zeynep Kılıç",
                    Product = "Tablet",
                    Amount = 5000,
                    Status = "İletişimde",
                    CreatedAt = DateTime.UtcNow,
                    LastStatusChange = DateTime.UtcNow,
                    Notes = new List<SaleNote>()
                }
            };

            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(sales);

            var result = await _saleService.GetAllSalesAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Ahmet Yılmaz", result[0].CustomerName);
            Assert.Equal("Zeynep Kılıç", result[1].CustomerName);
        }
        [Fact]
        public async Task AddNoteAsync_ValidSale_AddsNoteAndReturnsTrue()
        {
            var saleId = Guid.NewGuid().ToString();
            var sale = new Sale
            {
                Id = saleId,
                CustomerName = "Test Müşteri",
                Product = "Ürün",
                Amount = 12000,
                Status = "Yeni",
                Notes = new List<SaleNote>(),
                CreatedAt = DateTime.UtcNow,
                LastStatusChange = DateTime.UtcNow
            };

            var noteDto = new SaleNoteDto { Note = "Görüşme yapıldı." };

            _mockRepo.Setup(r => r.GetByIdAsync(saleId)).ReturnsAsync(sale);
            _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Sale>())).ReturnsAsync(true);

            var result = await _saleService.AddNoteAsync(saleId, noteDto);

            Assert.True(result);
            Assert.Single(sale.Notes);
            Assert.Equal("Görüşme yapıldı.", sale.Notes[0].Note);
        }

        [Fact]
        public async Task UpdateStatusAsync_ValidSale_UpdatesStatusAndAddsOptionalNote()
        {
            var saleId = Guid.NewGuid().ToString();
            var oldStatus = "İletişimde";
            var newStatus = "Anlaşma";

            var sale = new Sale
            {
                Id = saleId,
                CustomerName = "Test Müşteri",
                Product = "Ürün X",
                Amount = 15000,
                Status = oldStatus,
                Notes = new List<SaleNote>(),
                CreatedAt = DateTime.UtcNow,
                LastStatusChange = DateTime.UtcNow
            };

            var updateDto = new SaleUpdateStatusDto
            {
                NewStatus = newStatus,
                Note = "Satış tamamlandı"
            };

            _mockRepo.Setup(r => r.GetByIdAsync(saleId)).ReturnsAsync(sale);
            _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Sale>())).ReturnsAsync(true);

            var result = await _saleService.UpdateStatusAsync(saleId, updateDto);

            Assert.True(result);
            Assert.Equal(newStatus, sale.Status);
            Assert.Single(sale.Notes);
            Assert.Equal("Satış tamamlandı", sale.Notes[0].Note);
        }

    }
}

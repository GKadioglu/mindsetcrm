using SalesService.Application.Constants;
using SalesService.Application.Interfaces;
using SalesService.Domain.DTOs;
using SalesService.Domain.Entities;
using SalesService.Infrastructure.Interfaces;

namespace SalesService.Application.Services
{
    public class SaleService : ISaleService
    {
        private readonly ISaleRepository _repository;

        public SaleService(ISaleRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<SaleResponseDto>> GetAllSalesAsync()
        {
            var sales = await _repository.GetAllAsync();

            return sales.Select(s => new SaleResponseDto
            {
                Id = s.Id,
                CustomerName = s.CustomerName,
                Product = s.Product,
                Amount = s.Amount,
                Status = s.Status,
                Notes = s.Notes,
                CreatedAt = s.CreatedAt,
                LastStatusChange = s.LastStatusChange
            }).ToList();
        }

        public async Task<SaleResponseDto> GetSaleByIdAsync(string id)
        {
            var sale = await _repository.GetByIdAsync(id);
            if (sale == null)
                throw new Exception(ErrorMessages.SaleNotFound);

            return new SaleResponseDto
            {
                Id = sale.Id,
                CustomerName = sale.CustomerName,
                Product = sale.Product,
                Amount = sale.Amount,
                Status = sale.Status,
                Notes = sale.Notes,
                CreatedAt = sale.CreatedAt,
                LastStatusChange = sale.LastStatusChange
            };
        }

        public async Task<SaleResponseDto> CreateSaleAsync(SaleCreateDto dto)
        {
            var sale = new Sale
            {
                CustomerName = dto.CustomerName,
                Product = dto.Product,
                Amount = dto.Amount
            };

            await _repository.CreateAsync(sale);

            return new SaleResponseDto
            {
                Id = sale.Id,
                CustomerName = sale.CustomerName,
                Product = sale.Product,
                Amount = sale.Amount,
                Status = sale.Status,
                Notes = sale.Notes,
                CreatedAt = sale.CreatedAt,
                LastStatusChange = sale.LastStatusChange
            };
        }

        public async Task<SaleResponseDto> UpdateSaleAsync(string id, SaleUpdateDto dto)
        {
            var sale = await _repository.GetByIdAsync(id);
            if (sale == null)
                throw new Exception(ErrorMessages.SaleNotFound);

            sale.CustomerName = dto.CustomerName;
            sale.Product = dto.Product;
            sale.Amount = dto.Amount;

            await _repository.UpdateAsync(sale);

            return await GetSaleByIdAsync(id);
        }

        public async Task<bool> DeleteSaleAsync(string id)
        {
            var sale = await _repository.GetByIdAsync(id);
            if (sale == null)
                throw new Exception(ErrorMessages.SaleNotFound);

            return await _repository.DeleteAsync(id);
        }

        public async Task<bool> AddNoteAsync(string saleId, SaleNoteDto noteDto)
        {
            var sale = await _repository.GetByIdAsync(saleId);
            if (sale == null)
                throw new Exception(ErrorMessages.SaleNotFound);

            sale.Notes.Add(new SaleNote
            {
                Note = noteDto.Note,
                Date = DateTime.UtcNow
            });

            return await _repository.UpdateAsync(sale);
        }

        public async Task<bool> UpdateStatusAsync(string saleId, SaleUpdateStatusDto updateDto)
        {
            var sale = await _repository.GetByIdAsync(saleId);
            if (sale == null)
                throw new Exception(ErrorMessages.SaleNotFound);

            sale.Status = updateDto.NewStatus;
            sale.LastStatusChange = DateTime.UtcNow;

            if (!string.IsNullOrWhiteSpace(updateDto.Note))
            {
                sale.Notes.Add(new SaleNote
                {
                    Note = updateDto.Note,
                    Date = DateTime.UtcNow
                });
            }

            return await _repository.UpdateAsync(sale);
        }

        public async Task<bool> ChangeStatusAsync(string saleId, string newStatus)
        {
            var sale = await _repository.GetByIdAsync(saleId);
            if (sale == null)
                throw new Exception(ErrorMessages.SaleNotFound);

            sale.Status = newStatus;
            sale.LastStatusChange = DateTime.UtcNow;

            return await _repository.UpdateAsync(sale);
        }
    }
}

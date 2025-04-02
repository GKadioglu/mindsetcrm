using SalesService.Domain.DTOs;

namespace SalesService.Application.Interfaces
{
    public interface ISaleService
    {
        Task<List<SaleResponseDto>> GetAllSalesAsync();
        Task<SaleResponseDto> GetSaleByIdAsync(string id);
        Task<SaleResponseDto> CreateSaleAsync(SaleCreateDto createDto);
        Task<bool> UpdateStatusAsync(string saleId, SaleUpdateStatusDto updateDto);
        Task<SaleResponseDto> UpdateSaleAsync(string id, SaleUpdateDto updateDto);
        Task<bool> DeleteSaleAsync(string id);
        Task<bool> AddNoteAsync(string saleId, SaleNoteDto noteDto);
        Task<bool> ChangeStatusAsync(string saleId, string newStatus);
    }
}
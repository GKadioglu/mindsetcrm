using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerService.Domain.DTOs.Note;

namespace CustomerService.Application.Interfaces
{
    public interface ICustomerNoteService
    {
        Task<List<CustomerNoteResponseDto>> GetNotesByCustomerIdAsync(Guid customerId);
        Task<CustomerNoteResponseDto> GetNoteByIdAsync(Guid noteId);
        Task<CustomerNoteResponseDto> CreateNoteAsync(CustomerNoteCreateDto createDto);
        Task<CustomerNoteResponseDto> UpdateNoteAsync(Guid noteId, CustomerNoteUpdateDto updateDto);
        Task<bool> DeleteNoteAsync(Guid noteId);
    }
}
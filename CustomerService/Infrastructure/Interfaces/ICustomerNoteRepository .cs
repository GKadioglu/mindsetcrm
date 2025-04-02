using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerService.Domain.Entities;

namespace CustomerService.Infrastructure.Interfaces
{
    public interface ICustomerNoteRepository
    {
        Task<List<CustomerNote>> GetNotesByCustomerIdAsync(Guid customerId);
        Task<CustomerNote> GetNoteByIdAsync(Guid noteId);
        Task CreateNoteAsync(CustomerNote note);
        Task UpdateNoteAsync(CustomerNote note);
        Task<bool> DeleteNoteAsync(Guid noteId);
    }
}
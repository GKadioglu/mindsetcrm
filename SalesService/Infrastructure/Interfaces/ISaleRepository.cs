using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesService.Domain.Entities;

namespace SalesService.Infrastructure.Interfaces
{
    public interface ISaleRepository
    {
        Task<List<Sale>> GetAllAsync();
        Task<Sale> GetByIdAsync(string id);
        Task CreateAsync(Sale sale);
        Task<bool> UpdateAsync(Sale sale);
        Task<bool> DeleteAsync(string id);
    }
}
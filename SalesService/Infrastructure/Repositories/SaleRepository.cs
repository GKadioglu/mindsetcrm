using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using SalesService.Domain.Entities;
using SalesService.Infrastructure.Interfaces;
using SalesService.Infrastructure.MongoDBConnection;

namespace SalesService.Infrastructure.Repositories
{
    public class SaleRepository: ISaleRepository
    {
        private readonly IMongoCollection<Sale> _sales;

        public SaleRepository(MongoDbContext context)
        {
            _sales = context.Sales;
        }

        public async Task<List<Sale>> GetAllAsync()
        {
            return await _sales.Find(_ => true).ToListAsync();
        }

        public async Task<Sale> GetByIdAsync(string id)
        {
            return await _sales.Find(s => s.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Sale sale)
        {
            await _sales.InsertOneAsync(sale);
        }

        public async Task<bool> UpdateAsync(Sale sale)
        {
            var result = await _sales.ReplaceOneAsync(s => s.Id == sale.Id, sale);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _sales.DeleteOneAsync(s => s.Id == id);
            return result.DeletedCount > 0;
        }
    }
}
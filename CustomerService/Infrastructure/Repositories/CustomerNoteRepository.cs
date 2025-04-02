using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerService.Domain.Entities;
using CustomerService.Infrastructure.DbContext;
using CustomerService.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Infrastructure.Repositories
{
    public class CustomerNoteRepository : ICustomerNoteRepository
    {
        private readonly CustomerDbContext _context;

        public CustomerNoteRepository(CustomerDbContext context)
        {
            _context = context;
        }

        public async Task<List<CustomerNote>> GetNotesByCustomerIdAsync(Guid customerId)
        {
            return await _context.CustomerNotes
                .Where(n => n.CustomerId == customerId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<CustomerNote> GetNoteByIdAsync(Guid noteId)
        {
            return await _context.CustomerNotes.FirstOrDefaultAsync(n => n.Id == noteId);
        }

        public async Task CreateNoteAsync(CustomerNote note)
        {
            _context.CustomerNotes.Add(note);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateNoteAsync(CustomerNote note)
        {
            _context.CustomerNotes.Update(note);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteNoteAsync(Guid noteId)
        {
            var note = await _context.CustomerNotes.FindAsync(noteId);
            if (note == null)
                return false;

            _context.CustomerNotes.Remove(note);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
using CustomerService.Application.Constants;
using CustomerService.Application.Interfaces;
using CustomerService.Domain.DTOs.Note;
using CustomerService.Domain.Entities;
using CustomerService.Infrastructure.Interfaces;

namespace CustomerService.Application.Services
{
    public class CustomerNoteService : ICustomerNoteService
    {
        private readonly ICustomerNoteRepository _noteRepository;

        public CustomerNoteService(ICustomerNoteRepository noteRepository)
        {
            _noteRepository = noteRepository;
        }

        public async Task<List<CustomerNoteResponseDto>> GetNotesByCustomerIdAsync(Guid customerId)
        {
            var notes = await _noteRepository.GetNotesByCustomerIdAsync(customerId);

            return notes.Select(n => new CustomerNoteResponseDto
            {
                Id = n.Id,
                CustomerId = n.CustomerId,
                Note = n.Note,
                CreatedAt = n.CreatedAt
            }).ToList();
        }

        public async Task<CustomerNoteResponseDto> GetNoteByIdAsync(Guid noteId)
        {
            var note = await _noteRepository.GetNoteByIdAsync(noteId);
            if (note == null)
                throw new Exception(ErrorMessages.CustomerNoteNotFound);

            return new CustomerNoteResponseDto
            {
                Id = note.Id,
                CustomerId = note.CustomerId,
                Note = note.Note,
                CreatedAt = note.CreatedAt
            };
        }

        public async Task<CustomerNoteResponseDto> CreateNoteAsync(CustomerNoteCreateDto createDto)
        {
            var note = new CustomerNote
            {
                Id = Guid.NewGuid(),
                CustomerId = createDto.CustomerId,
                Note = createDto.Note,
                CreatedAt = DateTime.UtcNow
            };

            await _noteRepository.CreateNoteAsync(note);

            return new CustomerNoteResponseDto
            {
                Id = note.Id,
                CustomerId = note.CustomerId,
                Note = note.Note,
                CreatedAt = note.CreatedAt
            };
        }

        public async Task<CustomerNoteResponseDto> UpdateNoteAsync(Guid noteId, CustomerNoteUpdateDto updateDto)
        {
            var note = await _noteRepository.GetNoteByIdAsync(noteId);
            if (note == null)
                throw new Exception(ErrorMessages.CustomerNoteNotFound);

            note.Note = updateDto.Note;

            await _noteRepository.UpdateNoteAsync(note);

            return new CustomerNoteResponseDto
            {
                Id = note.Id,
                CustomerId = note.CustomerId,
                Note = note.Note,
                CreatedAt = note.CreatedAt
            };
        }

        public async Task<bool> DeleteNoteAsync(Guid noteId)
        {
            var note = await _noteRepository.GetNoteByIdAsync(noteId);
            if (note == null)
                throw new Exception(ErrorMessages.CustomerNoteNotFound);

            return await _noteRepository.DeleteNoteAsync(noteId);
        }
    }
}

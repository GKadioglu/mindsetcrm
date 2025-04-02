using Xunit;
using Moq;
using System;
using System.Threading.Tasks;
using CustomerService.Domain.Entities;
using CustomerService.Domain.DTOs.Note;
using CustomerService.Application.Services;
using CustomerService.Infrastructure.Interfaces;
using System.Collections.Generic;
using CustomerService.Application.Constants;

namespace CustomerService.Tests.Services
{
    public class CustomerNoteServiceTests
    {
        private readonly Mock<ICustomerNoteRepository> _mockRepo;
        private readonly CustomerNoteService _noteService;

        public CustomerNoteServiceTests()
        {
            _mockRepo = new Mock<ICustomerNoteRepository>();
            _noteService = new CustomerNoteService(_mockRepo.Object);
        }

        [Fact]
        public async Task CreateNoteAsync_ValidData_ReturnsNoteResponse()
        {
            var createDto = new CustomerNoteCreateDto
            {
                CustomerId = Guid.NewGuid(),
                Note = "Test notu"
            };

            _mockRepo.Setup(r => r.CreateNoteAsync(It.IsAny<CustomerNote>()))
                     .Returns(Task.CompletedTask);

            var result = await _noteService.CreateNoteAsync(createDto);

            Assert.NotNull(result);
            Assert.Equal(createDto.CustomerId, result.CustomerId);
            Assert.Equal(createDto.Note, result.Note);
            Assert.True(result.CreatedAt <= DateTime.UtcNow);
        }

        [Fact]
        public async Task GetNotesByCustomerIdAsync_ShouldReturnNotes()
        {
            var customerId = Guid.NewGuid();
            var notes = new List<CustomerNote>
            {
                new CustomerNote { Id = Guid.NewGuid(), CustomerId = customerId, Note = "Not 1", CreatedAt = DateTime.UtcNow },
                new CustomerNote { Id = Guid.NewGuid(), CustomerId = customerId, Note = "Not 2", CreatedAt = DateTime.UtcNow }
            };

            _mockRepo.Setup(r => r.GetNotesByCustomerIdAsync(customerId)).ReturnsAsync(notes);

            var result = await _noteService.GetNotesByCustomerIdAsync(customerId);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, r => Assert.Equal(customerId, r.CustomerId));
        }

        [Fact]
        public async Task GetNoteByIdAsync_InvalidId_ThrowsException()
        {
            var noteId = Guid.NewGuid();
            _mockRepo.Setup(r => r.GetNoteByIdAsync(noteId)).ReturnsAsync((CustomerNote?)null);

            var ex = await Assert.ThrowsAsync<Exception>(() => _noteService.GetNoteByIdAsync(noteId));

            Assert.Equal(ErrorMessages.CustomerNoteNotFound, ex.Message);
        }

        [Fact]
        public async Task UpdateNoteAsync_ValidId_UpdatesNote()
        {
            var noteId = Guid.NewGuid();
            var existingNote = new CustomerNote
            {
                Id = noteId,
                CustomerId = Guid.NewGuid(),
                Note = "Eski not",
                CreatedAt = DateTime.UtcNow
            };

            var updateDto = new CustomerNoteUpdateDto
            {
                Note = "Güncel not"
            };

            _mockRepo.Setup(r => r.GetNoteByIdAsync(noteId)).ReturnsAsync(existingNote);
            _mockRepo.Setup(r => r.UpdateNoteAsync(It.IsAny<CustomerNote>())).Returns(Task.CompletedTask);

            var result = await _noteService.UpdateNoteAsync(noteId, updateDto);

            Assert.NotNull(result);
            Assert.Equal(updateDto.Note, result.Note);
        }

        [Fact]
        public async Task UpdateNoteAsync_InvalidId_ThrowsException()
        {
            var noteId = Guid.NewGuid();
            _mockRepo.Setup(r => r.GetNoteByIdAsync(noteId)).ReturnsAsync((CustomerNote?)null);

            var updateDto = new CustomerNoteUpdateDto { Note = "Test" };

            var ex = await Assert.ThrowsAsync<Exception>(() => _noteService.UpdateNoteAsync(noteId, updateDto));

            Assert.Equal(ErrorMessages.CustomerNoteNotFound, ex.Message);
        }

        [Fact]
        public async Task DeleteNoteAsync_ValidId_DeletesNote()
        {
            var noteId = Guid.NewGuid();

            _mockRepo.Setup(r => r.GetNoteByIdAsync(noteId)).ReturnsAsync(new CustomerNote { Id = noteId });
            _mockRepo.Setup(r => r.DeleteNoteAsync(noteId)).ReturnsAsync(true);

            var result = await _noteService.DeleteNoteAsync(noteId);

            Assert.True(result);
        }

        [Fact]
        public async Task DeleteNoteAsync_InvalidId_ThrowsException()
        {
            var noteId = Guid.NewGuid();
            _mockRepo.Setup(r => r.GetNoteByIdAsync(noteId)).ReturnsAsync((CustomerNote?)null);

            var ex = await Assert.ThrowsAsync<Exception>(() => _noteService.DeleteNoteAsync(noteId));

            Assert.Equal(ErrorMessages.CustomerNoteNotFound, ex.Message);
        }
    }
}

using CustomerService.Application.Interfaces;
using CustomerService.Domain.DTOs.Note;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomerService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerNotesController : ControllerBase
    {
        private readonly ICustomerNoteService _noteService;

        public CustomerNotesController(ICustomerNoteService noteService)
        {
            _noteService = noteService;
        }

        [HttpGet("by-customer/{customerId}")]
        public async Task<ActionResult<List<CustomerNoteResponseDto>>> GetNotesByCustomerId(Guid customerId)
        {
            var notes = await _noteService.GetNotesByCustomerIdAsync(customerId);
            return Ok(notes);
        }

        [HttpGet("{noteId}")]
        public async Task<ActionResult<CustomerNoteResponseDto>> GetNoteById(Guid noteId)
        {
            try
            {
                var note = await _noteService.GetNoteByIdAsync(noteId);
                return Ok(note);
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<CustomerNoteResponseDto>> CreateNote([FromBody] CustomerNoteCreateDto createDto)
        {
            var createdNote = await _noteService.CreateNoteAsync(createDto);
            return CreatedAtAction(nameof(GetNoteById), new { noteId = createdNote.Id }, createdNote);
        }

        [HttpPut("{noteId}")]
        public async Task<ActionResult<CustomerNoteResponseDto>> UpdateNote(Guid noteId, [FromBody] CustomerNoteUpdateDto updateDto)
        {
            try
            {
                var updatedNote = await _noteService.UpdateNoteAsync(noteId, updateDto);
                return Ok(updatedNote);
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpDelete("{noteId}")]
        public async Task<ActionResult> DeleteNote(Guid noteId)
        {
            try
            {
                await _noteService.DeleteNoteAsync(noteId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }
    }
}

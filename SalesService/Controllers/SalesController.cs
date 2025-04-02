using Microsoft.AspNetCore.Mvc;
using SalesService.Application.Constants;
using SalesService.Application.Interfaces;
using SalesService.Domain.DTOs;

namespace SalesService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController : ControllerBase
    {
        private readonly ISaleService _saleService;

        public SalesController(ISaleService saleService)
        {
            _saleService = saleService;
        }

        [HttpGet]
        public async Task<ActionResult<List<SaleResponseDto>>> GetAll()
        {
            var sales = await _saleService.GetAllSalesAsync();
            return Ok(sales);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SaleResponseDto>> GetById(string id)
        {
            try
            {
                var sale = await _saleService.GetSaleByIdAsync(id);
                return Ok(sale);
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<SaleResponseDto>> Create([FromBody] SaleCreateDto dto)
        {
            var created = await _saleService.CreateSaleAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SaleResponseDto>> Update(string id, [FromBody] SaleUpdateDto dto)
        {
            try
            {
                var updated = await _saleService.UpdateSaleAsync(id, dto);
                return Ok(updated);
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                await _saleService.DeleteSaleAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpPost("{id}/notes")]
        public async Task<ActionResult> AddNote(string id, [FromBody] SaleNoteDto noteDto)
        {
            try
            {
                await _saleService.AddNoteAsync(id, noteDto);
                return Ok(new { message = SuccessMessages.SaleNoteAdded });
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpPatch("{id}/status")]
        public async Task<ActionResult> ChangeStatus(string id, [FromQuery] string newStatus)
        {
            try
            {
                await _saleService.ChangeStatusAsync(id, newStatus);
                return Ok(new { message = SuccessMessages.SaleStatusChanged });
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(string id, [FromBody] SaleUpdateStatusDto updateDto)
        {
            try
            {
                await _saleService.UpdateStatusAsync(id, updateDto);
                return Ok(new { message = SuccessMessages.SaleStatusChanged });
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }
    }
}

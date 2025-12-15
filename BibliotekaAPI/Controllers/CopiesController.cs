using BibliotekaAPI.Data;
using BibliotekaAPI.DTOs;
using BibliotekaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BibliotekaAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CopiesController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public CopiesController(LibraryDbContext context)
        {
            _context = context;
        }

        private static CopyOutputDto MapToOutputDto(Copy copy)
        {
            return new CopyOutputDto
            {
                Id = copy.Id,
                Status = copy.Status,
                BookId = copy.BookId,
                BookTitle = copy.Book.Title 
            };
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CopyOutputDto>>> GetCopies()
        {
            var copies = await _context.Copies
                                         .Include(c => c.Book)
                                         .ToListAsync();

            var copyDtos = copies.Select(MapToOutputDto).ToList();

            return Ok(copyDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CopyOutputDto>> GetCopy(int id)
        {
            var copy = await _context.Copies
                .Include(c => c.Book) 
                .FirstOrDefaultAsync(c => c.Id == id);

            if (copy == null)
            {
                return NotFound();
            }

            return Ok(MapToOutputDto(copy));
        }

        [HttpPost]
        public async Task<ActionResult<CopyOutputDto>> CreateCopy(CopyCreateUpdateDto copyDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var bookExists = await _context.Books.AnyAsync(b => b.Id == copyDto.BookId);
            if (!bookExists)
            {
                return BadRequest($"Książka o ID {copyDto.BookId} nie istnieje.");
            }

            var copy = new Copy
            {
                BookId = copyDto.BookId,
                Status = copyDto.Status ?? "Dostępny" // Używamy domyślnego statusu, jeśli nie podano
            };

            _context.Copies.Add(copy);
            await _context.SaveChangesAsync();

            var createdCopy = await _context.Copies
                                             .Include(c => c.Book)
                                             .FirstAsync(c => c.Id == copy.Id);

            return CreatedAtAction(nameof(GetCopy), new { id = createdCopy.Id }, MapToOutputDto(createdCopy));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCopy(int id, CopyCreateUpdateDto copyDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bookExists = await _context.Books.AnyAsync(b => b.Id == copyDto.BookId);
            if (!bookExists)
            {
                return BadRequest($"Książka o ID {copyDto.BookId} nie istnieje.");
            }

            var copy = await _context.Copies.FindAsync(id);
            if (copy == null)
            {
                return NotFound();
            }

            copy.BookId = copyDto.BookId;
            copy.Status = copyDto.Status ?? copy.Status; 

            try
            {
                _context.Update(copy);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Copies.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCopy(int id)
        {
            var copy = await _context.Copies.FindAsync(id);
            if (copy == null)
            {
                return NotFound();
            }

            _context.Copies.Remove(copy);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

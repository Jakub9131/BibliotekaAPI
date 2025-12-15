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
    public class AuthorsController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public AuthorsController(LibraryDbContext context)
        {
            _context = context;
        }

        // GET: /authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAuthors()
        {
            var authors = await _context.Authors.ToListAsync();

            var authorDtos = authors.Select(a => new AuthorDto
            {
                Id = a.Id,
                FirstName = a.FirstName,
                // KOREKTA 5: Wymuszamy zwracanie "" zamiast null
                LastName = a.LastName ?? ""
            }).ToList();

            return Ok(authorDtos);
        }

        // GET: /authors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorDto>> GetAuthor(int id)
        {
            var author = await _context.Authors
                .FirstOrDefaultAsync(a => a.Id == id);

            if (author == null)
            {
                return NotFound();
            }

            var authorDto = new AuthorDto
            {
                Id = author.Id,
                FirstName = author.FirstName,
                LastName = author.LastName ?? ""
            };

            return Ok(authorDto);
        }

        // POST: /authors
        [HttpPost]
        public async Task<ActionResult<AuthorDto>> CreateAuthor(AuthorDto authorDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var author = new Author
            {
                FirstName = authorDto.FirstName,
                LastName = authorDto.LastName
            };

            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            authorDto.Id = author.Id;
            authorDto.LastName = authorDto.LastName ?? "";
            return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, authorDto);
        }

        // PUT: /authors/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor(int id, AuthorDto authorDto)
        {
            // KOREKTA 6: Obsługa int? Id (nullable) dla PUT, gdy ID nie jest w ciele JSON.
            // Sprawdzenie, czy Id w DTO jest podane (ma wartość) i jest różne od ID w URL.
            if (authorDto.Id.HasValue && id != authorDto.Id.Value)
            {
                return BadRequest("ID w adresie URL musi pasować do ID w ciele żądania.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            author.FirstName = authorDto.FirstName;
            author.LastName = authorDto.LastName;

            try
            {
                _context.Update(author);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Authors.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: /authors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            if (await _context.Books.AnyAsync(b => b.AuthorId == id))
            {
                return BadRequest("Nie można usunąć autora, który ma przypisane książki. Najpierw usuń powiązane książki.");
            }

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
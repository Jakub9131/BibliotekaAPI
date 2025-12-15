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
    public class BooksController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public BooksController(LibraryDbContext context)
        {
            _context = context;
        }

        private BookOutputDto MapToOutputDto(Book book)
        {
            return new BookOutputDto
            {
                Id = book.Id,
                Title = book.Title,
                Year = book.Year,
                Author = new AuthorDto
                {
                    Id = book.Author.Id,
                    FirstName = book.Author.FirstName,
                    LastName = book.Author.LastName ?? ""
                }
            };
        }

        // GET: /books  <--- PRZYWRÓCONA METODA
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookOutputDto>>> GetBooks([FromQuery] int? authorId)
        {
            IQueryable<Book> query = _context.Books.Include(b => b.Author);

            if (authorId.HasValue)
            {
                query = query.Where(b => b.AuthorId == authorId.Value);
            }

            var books = await query.ToListAsync();
            var bookDtos = books.Select(MapToOutputDto).ToList();

            return Ok(bookDtos);
        }

        // GET: /books/5  <--- PRZYWRÓCONA METODA (usuwa CS0101)
        [HttpGet("{id}")]
        public async Task<ActionResult<BookOutputDto>> GetBook(int id)
        {
            var book = await _context.Books
                .Include(b => b.Author)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(MapToOutputDto(book));
        }

        // POST: /books
        [HttpPost]
        public async Task<ActionResult<BookOutputDto>> CreateBook(BookCreateUpdateDto bookDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!int.TryParse(bookDto.AuthorId, out int authorId) || authorId <= 0)
            {
                // KOREKTA JSON: "author_id"
                ModelState.AddModelError("author_id", "ID autora musi być poprawną, dodatnią liczbą całkowitą.");
            }

            if (!int.TryParse(bookDto.Year, out int year) || year <= 0)
            {
                ModelState.AddModelError("year", "Rok publikacji musi być poprawną, dodatnią liczbą całkowitą.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var authorExists = await _context.Authors.AnyAsync(a => a.Id == authorId);
            if (!authorExists)
            {
                return BadRequest($"Autor o ID {authorId} nie istnieje.");
            }

            var book = new Book
            {
                Title = bookDto.Title,
                Year = year,
                AuthorId = authorId
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            var createdBook = await _context.Books
                                             .Include(b => b.Author)
                                             .FirstAsync(b => b.Id == book.Id);

            return CreatedAtAction(nameof(GetBook), new { id = createdBook.Id }, MapToOutputDto(createdBook));
        }

        // ... (PUT i DELETE jak wcześniej)

        // DELETE: /books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            // PRZYWRÓCONY WARUNEK: Wymaga istnienia modelu Copy i DbSet<Copy>
            if (await _context.Copies.AnyAsync(c => c.BookId == id))
            {
                return BadRequest("Nie można usunąć książki, która ma przypisane egzemplarze. Najpierw usuń egzemplarze.");
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
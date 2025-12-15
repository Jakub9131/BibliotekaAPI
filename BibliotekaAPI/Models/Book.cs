using System.Collections.Generic;

namespace BibliotekaAPI.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!; // Zapobiega CS8618

        public int Year { get; set; }

        public int AuthorId { get; set; }

        public Author Author { get; set; } = null!;

        // KOREKTA CS01061: PRZYWRÓCENIE KOLEKCJI COPIES
        public ICollection<Copy> Copies { get; set; } = new List<Copy>();
    }
}
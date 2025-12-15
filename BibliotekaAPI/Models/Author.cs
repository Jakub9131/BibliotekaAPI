using System.ComponentModel.DataAnnotations;

namespace BibliotekaAPI.Models
{
    public class Author
    {
        public int Id { get; set; }

        // Imię pozostaje wymagane
        [Required(ErrorMessage = "Imię autora jest wymagane.")]
        public string FirstName { get; set; } = null!;

        // ZMIANA: Usunięto [Required] i dodano '?' aby było opcjonalne w DB
        public string? LastName { get; set; }

        // Relacja 1:N (Jeden Autor ma wiele Książek)
        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
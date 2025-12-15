using System.ComponentModel.DataAnnotations;

namespace BibliotekaAPI.Models
{
    public class Author
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Imię autora jest wymagane.")]
        public string FirstName { get; set; } = null!;

        public string? LastName { get; set; }

        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}

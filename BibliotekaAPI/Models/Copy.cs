using System.ComponentModel.DataAnnotations;

namespace BibliotekaAPI.Models
{
    public class Copy
    {
        public int Id { get; set; }

        public string Status { get; set; } = "Dostępny";

        public int BookId { get; set; } 
        public Book Book { get; set; } = null!; 
    }
}

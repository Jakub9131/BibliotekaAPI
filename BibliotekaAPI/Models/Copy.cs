using System.ComponentModel.DataAnnotations;

namespace BibliotekaAPI.Models
{
    public class Copy
    {
        public int Id { get; set; }

        // Dodatkowe opcjonalne pole, np. status egzemplarza
        public string Status { get; set; } = "Dostępny";

        // Relacja N:1 (Wiele Egzemplarzy ma jedną Książkę)
        public int BookId { get; set; } // Foreign Key
        public Book Book { get; set; } = null!; // Obiekt nawigacyjny
    }
}
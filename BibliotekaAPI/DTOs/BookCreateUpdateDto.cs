using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BibliotekaAPI.DTOs
{
    public class BookCreateUpdateDto
    {
        [Required(ErrorMessage = "Tytuł książki jest wymagany.")]
        // Brak JsonPropertyName, oczekuje "Title" lub "title"
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Rok publikacji jest wymagany.")]
        // Brak JsonPropertyName, oczekuje "Year" lub "year"
        public string Year { get; set; } = null!;

        // KOREKTA: Brak JsonPropertyName. Oczekuje "AuthorId" lub "authorId" (camelCase).
        // (W przeciwnym razie deserializacja zawsze zawiedzie)
        [Required(ErrorMessage = "ID autora jest wymagane.")]
        public string AuthorId { get; set; } = null!;
    }
}
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BibliotekaAPI.DTOs
{
    public class CopyCreateUpdateDto
    {
        [Required(ErrorMessage = "ID książki jest wymagane.")]
        [Range(1, int.MaxValue, ErrorMessage = "ID książki musi być dodatnie.")]
        // KOREKTA: Zmiana na snake_case, aby dopasować do standardu testów JS
        [JsonPropertyName("book_id")]
        public int BookId { get; set; }

        // Opcjonalny status
        [JsonPropertyName("status")]
        public string? Status { get; set; }
    }
}
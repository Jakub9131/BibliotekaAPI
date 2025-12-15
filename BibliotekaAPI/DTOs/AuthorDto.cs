using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BibliotekaAPI.DTOs
{
    public class AuthorDto
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Imię jest wymagane.")]
        [JsonPropertyName("first_name")] // Zgodne z testem JS
        public string FirstName { get; set; } = null!;

        // KOREKTA: Wymusza Minimalną długość 1, aby puste nazwisko ("") zwróciło 400.
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Nazwisko autora musi mieć co najmniej 1 znak.")]
        [JsonPropertyName("last_name")] // Zgodne z testem JS
        public string? LastName { get; set; }
    }
}
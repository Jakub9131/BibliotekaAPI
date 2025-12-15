using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BibliotekaAPI.DTOs
{
    public class CopyCreateUpdateDto
    {
        [Required(ErrorMessage = "ID książki jest wymagane.")]
        [Range(1, int.MaxValue, ErrorMessage = "ID książki musi być dodatnie.")]
        [JsonPropertyName("book_id")]
        public int BookId { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; }
    }
}

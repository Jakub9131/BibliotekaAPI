using System.Text.Json.Serialization;

namespace BibliotekaAPI.DTOs
{
    public class CopyOutputDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; } = "Dostępny";

        [JsonPropertyName("book_id")]
        public int BookId { get; set; }

        [JsonPropertyName("book_title")]
        public string BookTitle { get; set; } = null!;
    }
}

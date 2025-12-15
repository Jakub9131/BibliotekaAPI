using BibliotekaAPI.DTOs;// Niezbędne, aby używać AuthorDto
using System.Text.Json.Serialization; // WAŻNE!

namespace BibliotekaAPI.DTOs
{
    public class BookOutputDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; } = null!;

        [JsonPropertyName("year")]
        public int Year { get; set; } 

        [JsonPropertyName("author")] // Zagnieżdżony obiekt musi być "author"
        public AuthorDto Author { get; set; } = null!; // Pełny obiekt autora
    }
}
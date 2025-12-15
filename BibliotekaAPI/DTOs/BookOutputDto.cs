using BibliotekaAPI.DTOs;
using System.Text.Json.Serialization;

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

        [JsonPropertyName("author")] 
        public AuthorDto Author { get; set; } = null!; 
    }
}

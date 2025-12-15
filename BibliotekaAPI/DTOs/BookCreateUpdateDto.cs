using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BibliotekaAPI.DTOs
{
    public class BookCreateUpdateDto
    {
        [Required(ErrorMessage = "Tytuł książki jest wymagany.")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Rok publikacji jest wymagany.")]
        public string Year { get; set; } = null!;

        [Required(ErrorMessage = "ID autora jest wymagane.")]
        public string AuthorId { get; set; } = null!;
    }
}

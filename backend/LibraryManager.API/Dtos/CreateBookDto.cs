using System.ComponentModel.DataAnnotations;

namespace LibraryManager.API.Dtos
{
    /// <summary>
    /// Data Transfer Object for CreateBook entity.
    /// </summary>
    public class CreateBookDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public int AuthorId { get; set; }

        [Required]
        public int PublisherId { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace LibraryManager.API.Dtos
{
    /// <summary>
    /// Data Transfer Object for UpdateBook entity.
    /// </summary>
    public class UpdateBookDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public int AuthorId { get; set; }
        [Required]
        public int PublisherId { get; set; }
    }
}

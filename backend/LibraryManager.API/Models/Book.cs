namespace LibraryManager.API.Models
{
    /// <summary>
    /// Represents an author entity in the library system.
    /// </summary>
    public class Book
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int AuthorId { get; set; }
        public Author Author { get; set; }
        public int PublisherId { get; set; }
        public Publisher Publisher { get; set; }
        public bool IsBorrowed { get; set; }
    }
}

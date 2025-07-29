namespace LibraryManager.API.Dtos
{
    /// <summary>
    /// Data Transfer Object for Book entity.
    /// </summary>
    public class BookDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public AuthorDto Author { get; set; }
        public PublisherDto Publisher { get; set; }
        public bool IsBorrowed { get; set; }
    }
}
